using UnityEngine;

namespace UnityGameStarter.Math.Vector 
{
    public static class VectorLibrary
    {
        public static Vector3 ToVector3(this Vector2 vector, float z = 0)
            => new(vector.x, vector.y, z);

        public static Vector2 ToVector2XZ(this Vector3 vector)
            => new(vector.x, vector.z);

        public static Vector3 ToVector3XZ(this Vector2 vector, float y = 0)
            => new(vector.x, y, vector.y);

        public static Vector2Int ToVector2Int(this Vector2 vector, IntConversionMode mode = IntConversionMode.Floor) 
        {
            int x = StandardMathLibrary.IntConversion(vector.x, mode);
            int y = StandardMathLibrary.IntConversion(vector.y, mode);

            return new(x, y);
        }

        public static Vector3Int ToVector3Int(this Vector3 vector, IntConversionMode mode = IntConversionMode.Floor) 
        {
            int x = StandardMathLibrary.IntConversion(vector.x, mode);
            int y = StandardMathLibrary.IntConversion(vector.y, mode);
            int z = StandardMathLibrary.IntConversion(vector.z, mode);

            return new(x, y, z);
        }

        public static Vector2 Scale(this Vector2 vector, float magnitude)
            => vector * magnitude;

        public static Vector3 Scale(this Vector3 vector, float magnitude)
            => vector * magnitude;

        public static Vector2 OffsetComponents(this Vector2 vector, float value)
            => new (vector.x + value, vector.y + value);

        public static Vector3 OffsetComponents(this Vector3 vector, float value)
            => new (vector.x + value, vector.y + value, vector.z + value);
    }
}

