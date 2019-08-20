using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maple.NetCore
{
    public interface IQuartzFactory
    {
        void Register(bool addJobs);

        void Start();

        void Stop();

        Task StartAsync();

        Task StopAsync();

        void AddJob(KeyValuePair<Type, CQuartzJobType> jobDetail);

        void AddRangeJob(IEnumerable<KeyValuePair<Type, CQuartzJobType>> jobDetails);

    }
}
