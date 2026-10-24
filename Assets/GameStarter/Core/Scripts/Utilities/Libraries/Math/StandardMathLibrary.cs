using System;
using UnityEngine;

namespace UnityGameStarter.Math 
{
    public enum IntConversionMode
    {
        Round,
        Floor,
        Ceil
    }

    public enum HalfToIntMode 
    {
        Default,
        Up,
        Down
    }

    public static class StandardMathLibrary 
    {
        public static int IntConversion(this float value, IntConversionMode mode = IntConversionMode.Floor) 
        {
            return mode switch
            {
                IntConversionMode.Round => Mathf.RoundToInt(value),
                IntConversionMode.Floor => Mathf.FloorToInt(value),
                IntConversionMode.Ceil => Mathf.CeilToInt(value),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }

        public static int IntConversion(this float value, HalfToIntMode him, 
            IntConversionMode icm = IntConversionMode.Floor)
        {
            return him switch
            {
                HalfToIntMode.Default => value.IntConversion(icm),
                HalfToIntMode.Up => value.HalfUpToInt(icm),
                HalfToIntMode.Down => value.HalfDownToInt(icm),
                _ => throw new ArgumentOutOfRangeException(nameof(him), him, null)
            };
        }

        public static int HalfUpToInt(this float value, IntConversionMode mode = IntConversionMode.Floor)
            => (value + 0.5f).IntConversion(mode);

        public static int HalfDownToInt(this float value, IntConversionMode mode = IntConversionMode.Floor)
            => (value - 0.5f).IntConversion(mode);
    }
}