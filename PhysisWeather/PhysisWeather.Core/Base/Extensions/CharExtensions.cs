using System;

namespace PhysisWeather.Core.Base.Extensions
{
    public static class CharExtensions
    {
        public static bool IsUpper(this char c)
        {
            return c >= 65 && c <= 90;
        }

        public static bool IsLower(this char c)
        {
            return c >= 97 && c <= 122;
        }

        public static bool IsDigit0Through9(this char c)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }

            return true;
        }
    }
}
