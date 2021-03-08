using System;

namespace PhysisWeather.Core.Base.Extensions
{
    public static class StringExtensions
    {
        public static string GetFirst(this string source, int numberOfChars)
        {
            if (source.Length > numberOfChars)
            {
                return source.Substring(0, numberOfChars);
            }

            return source;
        }

        public static string GetLast(this string source, int numberOfChars)
        {
            if (numberOfChars >= source.Length)
            {
                return source;
            }

            return source.Substring(source.Length - numberOfChars);
        }

        public static string GetAfterLastOrEmpty(this string text, string stopAt)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(stopAt))
            {
                int charLocation = text.LastIndexOf(stopAt) + 1;

                if (charLocation > 0)
                {
                    return text.Substring(charLocation, text.Length - charLocation);
                }
            }

            return string.Empty;
        }

        public static string GetUntilOrEmpty(this string text, string stopAt)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(stopAt))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return string.Empty;
        }
    }
}
