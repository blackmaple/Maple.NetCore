using NeoSmart.AsyncLock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maple.NetCore
{
    public interface IConcurrencyLockCore
    {
        AsyncLock.InnerLock GetLock(string lockName, int seconds = 128);

        Task<AsyncLock.InnerLock> GetLockAsync(string lockName, int seconds = 128);

    }
}
