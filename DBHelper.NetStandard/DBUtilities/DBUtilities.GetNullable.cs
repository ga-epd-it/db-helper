using System;

namespace EpdIt
{
    public static partial class DBUtilities
    {
        public static T GetNullable<T>(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
            {
                return default;
            }

            return (T)obj;
        }

        public static string GetNullableString(object obj)
        {
            return GetNullable<string>(obj);
        }

        public static DateTime? GetNullableDateTime(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
            {
                return default;
            }

            if (obj is DateTime result)
            {
                return result;
            }

            if (DateTime.TryParse(obj.ToString(), out DateTime parsed))
            {
                return parsed;
            }

            return default;
        }
    }
}
