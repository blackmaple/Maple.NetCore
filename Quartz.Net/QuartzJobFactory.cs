using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using Scrutor;
using System;

namespace Maple.NetCore
{
    internal class CQuartzJobFactory : SimpleJobFactory
    {
        IServiceProvider ServiceProvider { get; }
        public CQuartzJobFactory(IServiceProvider serviceProvider)
        {
            
            this.ServiceProvider = serviceProvider;
        }
        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            if (this.ServiceProvider.GetService(bundle.JobDetail.JobType) is IJob job)
            {
                return job;
            }
            return base.NewJob(bundle, scheduler);
        }

        public override void ReturnJob(IJob job)
        {
            //base.ReturnJob(job);
        }
    }
}
