using UnityEngine;

namespace UnityGameStarter.Math.Vector 
{
    public static class VectorLibrary
    {
        public static Vector2 Vector3ToVector2(Vector3 vector)
            => new(vector.x, vector.y);

        public static Vector3 Vector2ToVector3(Vector2 vector)
            => new(vector.x, vector.y, 0);

        public static Vector2Int ToVector2Int(Vector2 vector, IntConversionMode mode = IntConversionMode.Floor) 
        {
            int x = StandardMathLibrary.IntConversion(vector.x, mode);
            int y = StandardMathLibrary.IntConversion(vector.y, mode);

            return new(x, y);
        }

        public static Vector3Int ToVector3Int(Vector3 vector, IntConversionMode mode = IntConversionMode.Floor) 
        {
            int x = StandardMathLibrary.IntConversion(vector.x, mode);
            int y = StandardMathLibrary.IntConversion(vector.y, mode);
            int z = StandardMathLibrary.IntConversion(vector.z, mode);

            return new(x, y, z);
        }

        public static Vector2 Scale(Vector2 vector, float magnitude)
            => vector * magnitude;

        public static Vector3 Scale(Vector3 vector, float magnitude)
            => vector * magnitude;

        public static Vector2 OffsetComponents(Vector2 vector, float value)
            => Vector2.one * value + vector;

        public static Vector3 OffsetComponents(Vector3 vector, float value)
            => Vector3.one * value + vector;
    }
}

