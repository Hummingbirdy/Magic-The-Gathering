using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MTG.Utilities
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj, bool includeNull = true)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() },
                NullValueHandling = includeNull ? NullValueHandling.Include : NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        public static string ConvertEnumToJson(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("The conversion parameter must be an enumeration", nameof(enumType));

            var ret = "{";
            foreach (var val in Enum.GetValues(enumType))
            {
                var name = Enum.GetName(enumType, val);
                ret += name + ":" + ((int)val) + ",";
            }
            ret += "}";

            return ret;
        }

    }
}