using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public class CCommonEnumExtensions<T_Enum> where T_Enum : struct
    {
        static Dictionary<string, int> EnumProps { get; }

        static CCommonEnumExtensions()
        {
            var enumType = typeof(T_Enum);
            var fields = enumType.GetFields();
            EnumProps = new Dictionary<string, int>(fields.Length);
            foreach (var f in fields)
            {
                if (f.FieldType != enumType)
                {
                    continue;
                }
                var val = Convert.ToInt32(f.GetValue(default));
                EnumProps.Add(f.Name, val);
            }
        }

        public static bool TryGetValue(string key, out int val)
        {
            return EnumProps.TryGetValue(key, out val);
        }

        public static IReadOnlyDictionary<string, int> KeyValuePairs => EnumProps;
    }
}
