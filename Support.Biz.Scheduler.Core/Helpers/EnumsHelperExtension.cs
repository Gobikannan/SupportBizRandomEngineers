using System;
using System.ComponentModel;

namespace Support.Biz.Scheduler.Core
{
    public static class EnumsExtension
    {
        public static string ToDescription(this Enum value)
        {
            DescriptionAttribute[] da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}
