using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Document_Management_System.Models
{
    public static class StringHelper
    {
        public static bool IsUpper(this string value)
        {
            // Consider string to be uppercase if it has no lowercase letters.
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsLower(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsLower(this string value)
        {
            // Consider string to be lowercase if it has no uppercase letters.
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static string ToTitle(this string value)
        {
            //CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            //TextInfo textInfo = cultureInfo.TextInfo;
            //TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            //return textInfo.ToTitleCase(value); 

            string val = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            return val;
        }

    }
}