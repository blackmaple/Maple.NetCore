using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Maple.NetCore
{
    //[ServiceDescriptor(typeof(IQuartzFactory), ServiceLifetime.Singleton)]
    [CDISingleton(typeof(IQuartzFactory))]
    public class CQuartzFactory : IQuartzFactory
    {
        IScheduler Scheduler { get; }
        IApplicationLifetime ApplicationLifetime { get; }
        public CQuartzFactory(IServiceProvider serviceProvider, IApplicationLifetime lifetime)
        {
            var nameValue = new NameValueCollection()
            {
                ["quartz.scheduler.instanceName"] = "ExampleDefaultQuartzScheduler",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.serializer.type"] = "binary",
                ["quartz.threadPool.threadCount"] = "16",
                ["quartz.jobStore.misfireThreshold"] = "6000",
                ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz",

                //      ["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins",
                //      ["quartz.plugin.xml.fileNames"] = $@"{System.IO.Path.Combine(AppContext.BaseDirectory, "Config", "quartz_jobs.xml")}",
            };
            var factory = new StdSchedulerFactory(nameValue);
            this.Scheduler = factory.GetScheduler().Result;
            this.Scheduler.JobFactory = new CQuartzJobFactory(serviceProvider);
            this.ApplicationLifetime = lifetime;

        }


        public void Start()
        {
            "Start".DebugPrintf();
            this.StartAsync().Wait();
        }

        public void Stop()
        {
            "Stop".DebugPrintf();
            this.StopAsync().Wait();
        }

        public Task StartAsync()
        {
            return this.Scheduler.Start();
        }

        public Task StopAsync()
        {
            return this.Scheduler.Shutdown();
        }

        public void Register(bool addJobs)
        {
            if (addJobs)
            {
                this.AddRangeJob(CQuartzJobAttribute.DictionaryQuartzJob);
            }
            this.ApplicationLifetime.ApplicationStarted.Register(this.Start);
            this.ApplicationLifetime.ApplicationStopped.Register(this.Stop);
        }

        public void AddJob(KeyValuePair<Type, CQuartzJobType> jobDetail)
        {
            var jobType = jobDetail.Value;

            switch (jobType.EnumQuartzJobType)
            {
                default:
                case EnumQuartzJobType.None:
                case EnumQuartzJobType.WithCronSchedule:
                    {
                        var job = JobBuilder
                            .Create(jobDetail.Key)
                            .WithIdentity($@"Job:{jobDetail.Key.FullName}")
                            .Build();
                        var trigger = TriggerBuilder.Create()
                            .WithIdentity($@"Trigger:{jobDetail.Key.FullName}")
                            .WithCronSchedule(jobType.CronExpression)
                             .Build();
                        this.Scheduler.ScheduleJob(job, trigger);
                        break;
                    }
                case EnumQuartzJobType.WithSimpleSchedule:
                    {
                        for (int i = 0; i < jobType.ConcurrentCount; ++i)
                        {
                            var index = i.ToString("D8");
                            var job = JobBuilder
                                .Create(jobDetail.Key)
                                .WithIdentity($@"Job:{jobDetail.Key.FullName}_{index}")
                                .Build();

                            var trigger = TriggerBuilder.Create()
                                .WithIdentity($@"Trigger:{jobDetail.Key.FullName}_{index}")
                                .WithSimpleSchedule(p=>
                                    p.WithInterval(TimeSpan.FromMilliseconds(jobType.Milliseconds))
                                    .WithRepeatCount(jobType.RepeatCount)
                                    ).Build();
                            this.Scheduler.ScheduleJob(job, trigger);
                        }

                        break;
                    }
            }



        }

        public void AddRangeJob(IEnumerable<KeyValuePair<Type, CQuartzJobType>> jobDetails)
        {
            foreach (var data in jobDetails)
            {
                this.AddJob(data);
            }
        }
    }
}
