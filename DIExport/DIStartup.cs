using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

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
    }
}
