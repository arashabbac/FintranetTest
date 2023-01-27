using System;

namespace Framework.Extensions
{
    public static class StringExtension
    {
        static StringExtension()
        {
        }

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static string Fix(this string text)
        {
            if (text is null)
            {
                return null;
            }

            text = text.Trim();

            if (text == string.Empty)
            {
                return null;
            }

            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }

            return text;
        }

        public static bool IsJustInt(this string text)
        {
            if (text == null)
            {
                return false;
            }

            if (text == string.Empty)
            {
                return false;
            }

            try
            {
                Convert.ToInt32(text);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsJustLong(this string text)
        {
            if (text == null)
            {
                return false;
            }

            if (text == string.Empty)
            {
                return false;
            }

            try
            {
                System.Convert.ToInt64(text);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
