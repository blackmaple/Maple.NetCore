using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    /// <summary>
    /// 定时任务特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CQuartzJobAttribute : ServiceDescriptorAttribute
    {
        internal static ConcurrentDictionary<Type, CQuartzJobType> DictionaryQuartzJob { get; }
        static CQuartzJobAttribute()
        {
            DictionaryQuartzJob = new ConcurrentDictionary<Type, CQuartzJobType>(8, 64);
        }

        public CQuartzJobAttribute(Type jobType, string cronExpression) : base(jobType, ServiceLifetime.Transient)
        {
            DictionaryQuartzJob.TryAdd(jobType, new CQuartzJobType()
            {
                EnumQuartzJobType = EnumQuartzJobType.WithCronSchedule,
                CronExpression = cronExpression,
            });
        }

        public CQuartzJobAttribute(Type jobType, int milliseconds, int concurrentCount, int repeatCount) : base(jobType, ServiceLifetime.Transient)
        {
            DictionaryQuartzJob.TryAdd(jobType, new CQuartzJobType()
            {
                EnumQuartzJobType = EnumQuartzJobType.WithSimpleSchedule,
                ConcurrentCount = concurrentCount,
                Milliseconds = milliseconds,
                RepeatCount = repeatCount
            });
        }
    }
}
