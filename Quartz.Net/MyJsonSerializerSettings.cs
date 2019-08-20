using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Maple.NetCore.Quartz.Net
{
    public class NullToEmptyStringResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                    .Select(p =>
                    {
                        var jp = base.CreateProperty(p, memberSerialization);
                        jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                        return jp;
                    }).ToList();
        }
    }

    public class NullToEmptyStringValueProvider : IValueProvider
    {
        PropertyInfo MemberInfo { get; }
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            MemberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object result = MemberInfo.GetValue(target);
            if (MemberInfo.PropertyType == typeof(string) && result == null)
            {
                result = string.Empty;
            }
            return result;

        }

        public void SetValue(object target, object value)
        {
            MemberInfo.SetValue(target, value);
        }
    }

}
