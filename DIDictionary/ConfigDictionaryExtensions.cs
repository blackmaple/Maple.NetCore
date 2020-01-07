using Maple.NetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maple.NetCore
{
    /// <summary>
    /// 不区分大小写
    /// </summary>
    public class CMapleSystemConfigDictionary : Dictionary<string, string>
    {
        public CMapleSystemConfigDictionary(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase)
        {
        }
        public CMapleSystemConfigDictionary() : this(1024)
        {
        }
    }

    ///// <summary>
    ///// 依赖注入
    ///// </summary>
    //[CDISingleton(typeof(IConfigureOptions<CMapleSystemConfigDictionary>))]
    //public class CSystemConfigDictionary_DIConfigureOptions : CDIConfigureOptions<CMapleSystemConfigDictionary>
    //{
    //    public CSystemConfigDictionary_DIConfigureOptions(IConfiguration config) : base(config.GetSection(nameof(CMapleSystemConfigDictionary)))
    //    {
    //    }
    //}

    ///// <summary>
    ///// 依赖注入 自动更新
    ///// </summary>
    //[CDISingleton(typeof(IOptionsChangeTokenSource<CMapleSystemConfigDictionary>))]
    //public class CSystemConfigDictionary_DIOptionsChangeTokenSource : CDIOptionsChangeTokenSource<CMapleSystemConfigDictionary>
    //{
    //    public CSystemConfigDictionary_DIOptionsChangeTokenSource(IConfiguration config) : base(config.GetSection(nameof(CMapleSystemConfigDictionary)))
    //    {
    //    }
    //}

    /// <summary>
    /// 扩展
    /// </summary>
    public static class CConfigDictionaryExtensions
    {
        public static string GetValue(this IDictionary<string, string> config, [CallerMemberName] string keyName = "")
        {
            if (config == null)
            {
                return string.Empty;
            }
            return config.TryGetValue(keyName, out var log) ? log : string.Empty;

        }

        public static string GetValueAsString(this IDictionary<string, string> config, [CallerMemberName] string keyName = "")
        {
            return config.GetValue(keyName);
        }

        public static int GetValueAsInt(this IDictionary<string, string> config, [CallerMemberName] string keyName = "", int def = 0)
        {
            var content = config.GetValue(keyName);
            return int.TryParse(content, out var val) ? val : def;
        }

        public static long GetValueAsLong(this IDictionary<string, string> config, [CallerMemberName] string keyName = "", long def = 0L)
        {
            var content = config.GetValue(keyName);
            return long.TryParse(content, out var val) ? val : def;
        }

        public static ulong GetValueAsULong(this IDictionary<string, string> config, [CallerMemberName] string keyName = "", ulong def = 0UL)
        {
            var content = config.GetValue(keyName);
            return ulong.TryParse(content, out var val) ? val : def;
        }

        public static float GetValueAsFloat(this IDictionary<string, string> config, [CallerMemberName] string keyName = "", float def = 0F)
        {
            var content = config.GetValue(keyName);
            return float.TryParse(content, out var val) ? val : def;
        }

        public static double GetValueAsDouble(this IDictionary<string, string> config, [CallerMemberName] string keyName = "", double def = 0D)
        {
            var content = config.GetValue(keyName);
            return double.TryParse(content, out var val) ? val : def;

        }

        public static decimal GetValueAsDecimal(this IDictionary<string, string> config, [CallerMemberName] string keyName = "", decimal def = 0M)
        {
            var content = config.GetValue(keyName);
            return decimal.TryParse(content, out var val) ? val : def;
        }
    }


}
