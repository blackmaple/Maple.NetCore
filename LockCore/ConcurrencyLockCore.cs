using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NeoSmart.AsyncLock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maple.NetCore
{
    public class CConcurrencyLockCore : IConcurrencyLockCore
    {
        public IMemoryCache MemoryCache { get; }

        public CConcurrencyLockCore(IMemoryCache cache)
        {
            this.MemoryCache = cache;
        }

        public AsyncLock.InnerLock GetLock(string lockName, int seconds = 128)
        {
            return this.MemoryCache.GetLock(lockName, seconds);
        }

        public Task<AsyncLock.InnerLock> GetLockAsync(string lockName, int seconds = 128)
        {
            return this.MemoryCache.GetLockAsync(lockName, seconds);
        }
  
    }
}
