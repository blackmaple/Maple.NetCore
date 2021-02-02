using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public static class CJsonCore
    {
        private static JsonSerializerSettings JsonSettings { get; }

        static CJsonCore()
        {
            JsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                DateFormatString = "yyyy.MM.dd HH:mm:ss",
            };
        }

        public static string Object2Json(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj, JsonSettings);
            return json;
        }

        public static T Json2Object<T>(this string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json, JsonSettings);
            return obj;
        }
    }

}
