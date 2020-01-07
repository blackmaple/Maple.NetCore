using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Maple.NetCore
{
    public static class CDIStartup
    {
        public static IServiceCollection Register(this IServiceCollection services, params string[] typeNames)
        {
            var defContext = DependencyContext.Default;
            var assemblies = defContext.RuntimeLibraries.Where(
                p => typeNames.Contains(p.Type, StringComparer.OrdinalIgnoreCase)).
                SelectMany(p => p.GetDefaultAssemblyNames(defContext)).Select(Assembly.Load).ToArray();
            return services.Scan(scan => scan.FromAssemblies(assemblies).AddClasses().UsingAttributes());

        }

        public static IServiceCollection Register(this IServiceCollection services)
        {
            return services.Register("project");
        }

        public static IServiceCollection RegisterDBConfig(this IServiceCollection services, IConfiguration config)
        {
            return services.RegisterDBConfig<CMapleDBConnectionString>(config, nameof(CMapleDBConnectionString));
        }

        public static IServiceCollection RegisterDBConfig<DB_CONFIG>(this IServiceCollection services, IConfiguration config, string key) where DB_CONFIG : CMapleDBConnectionString
        {
            var dbConfig = config.GetSection(key);
            return services
                .AddSingleton(typeof(IConfigureOptions<DB_CONFIG>), new CDIConfigureOptions<DB_CONFIG>(dbConfig))
                .AddSingleton(typeof(IOptionsChangeTokenSource<DB_CONFIG>), new CDIOptionsChangeTokenSource<DB_CONFIG>(dbConfig));
        }

        public static IServiceCollection RegisterSystemConfig(this IServiceCollection services, IConfiguration config)
        {
            return services.RegisterSystemConfig<CMapleSystemConfigDictionary>(config, nameof(CMapleSystemConfigDictionary));
        }

        public static IServiceCollection RegisterSystemConfig<SYSTEM_CONFIG>(this IServiceCollection services, IConfiguration config, string key) where SYSTEM_CONFIG : CMapleSystemConfigDictionary
        {
            var dbConfig = config.GetSection(key);
            return services
                .AddSingleton(typeof(IConfigureOptions<SYSTEM_CONFIG>), new CDIConfigureOptions<SYSTEM_CONFIG>(dbConfig))
                .AddSingleton(typeof(IOptionsChangeTokenSource<SYSTEM_CONFIG>), new CDIOptionsChangeTokenSource<SYSTEM_CONFIG>(dbConfig));
        }

    }
}
