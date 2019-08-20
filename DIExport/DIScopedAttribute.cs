using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CDIScopedAttribute : ServiceDescriptorAttribute
    {
        public CDIScopedAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Scoped) { }
    }
}
