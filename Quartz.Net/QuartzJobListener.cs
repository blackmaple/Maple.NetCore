using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.NetCore
{
    internal class CQuartzJobListener : IJobListener
    {
        public string Name { get; }
        ILog Log { get; }

        public CQuartzJobListener()
        {
            this.Name = nameof(CQuartzJobListener);
            this.Log = new CLog4netFactory().GetLogger(this.Name);
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var name = context.JobDetail.Key.Name;
            var time = context.JobRunTime;
            this.Log.Info($@"{this.Name}=>{name}=>{time}...{nameof(JobExecutionVetoed)}");
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var name = context.JobDetail.Key.Name;
            var time = context.JobRunTime;
            this.Log.Info($@"{this.Name}=>{name}=>{time}...{nameof(JobToBeExecuted)}");
            return Task.CompletedTask;

        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            var name = context.JobDetail.Key.Name;
            var time = context.JobRunTime;
            this.Log.Info($@"{this.Name}=>{name}=>{time}...{nameof(JobWasExecuted)}");
            if (jobException != null)
            {
                this.Log.Error(jobException.GetAllError());
            }
            return Task.CompletedTask;
        }
    }
}
