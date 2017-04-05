namespace Goop.Math
{
    using System;

    public static class MathEx
    {
        public static double Clamp(this double value, double min, double max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

        public static bool IsValidDoubleValue(double value)
        {
            return (double.IsNaN(value) || double.IsInfinity(value)) == false;
        }
    }
}
