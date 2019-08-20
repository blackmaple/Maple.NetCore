using Scrutor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CDITransientAttribute :  ServiceDescriptorAttribute
    {
        public CDITransientAttribute(Type serviceType) : base(serviceType, Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient) { }
    }
}
