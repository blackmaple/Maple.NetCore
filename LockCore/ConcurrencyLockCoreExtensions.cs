using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NeoSmart.AsyncLock;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maple.NetCore
{
    public static class ConcurrencyLockCoreExtensions  
    {
        public static Task<AsyncLock.InnerLock> GetLockAsync(this IMemoryCache memoryCache, string lockName, int seconds = 128)
        {
            return memoryCache.GetAsyncLock(lockName, seconds).LockAsync();
        }

        public static AsyncLock.InnerLock GetLock(this IMemoryCache memoryCache, string lockName, int seconds = 128)
        {
            return memoryCache.GetAsyncLock(lockName, seconds).Lock();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private static AsyncLock GetAsyncLock(this IMemoryCache memoryCache, string lockName, int seconds = 128)
        {
            var realKeyName = GetRealLockName(lockName);
            var obj = memoryCache.GetOrCreate(realKeyName, p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromSeconds(seconds));
                return new AsyncLock();
            });
            return obj;
        }


        private static string GetRealLockName(string lockName) => $@"{nameof(ConcurrencyLockCoreExtensions)}:{lockName}";
 
    }
}
