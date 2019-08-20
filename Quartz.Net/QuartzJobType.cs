using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public class CQuartzJobType
    {
        public EnumQuartzJobType EnumQuartzJobType { set; get; }
        /// <summary>
        /// WithCronSchedule
        /// </summary>
        public string CronExpression { set; get; }
        /// <summary>
        /// WithSimpleSchedule
        /// </summary>
        public int Milliseconds { set; get; }
        /// <summary>
        /// WithSimpleSchedule
        /// </summary>
        public int ConcurrentCount { get; set; }
        /// <summary>
        /// WithSimpleSchedule
        /// </summary>
        public int RepeatCount { get; set; }

        public override string ToString()
        {
            return this.CronExpression;
        }

        public static implicit operator string(CQuartzJobType obj)
        {
            return obj.CronExpression;
        }

        public static implicit operator EnumQuartzJobType(CQuartzJobType obj)
        {
            return obj.EnumQuartzJobType;
        }
    }
}
