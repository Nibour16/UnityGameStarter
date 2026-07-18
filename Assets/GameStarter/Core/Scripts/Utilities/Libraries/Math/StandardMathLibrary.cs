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

    public static class StandardMathLibrary 
    {
        public static int IntConversion(float value, IntConversionMode mode = IntConversionMode.Floor) 
        {
            return mode switch
            {
                IntConversionMode.Round => Mathf.RoundToInt(value),
                IntConversionMode.Floor => Mathf.FloorToInt(value),
                IntConversionMode.Ceil => Mathf.CeilToInt(value),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
    }
}