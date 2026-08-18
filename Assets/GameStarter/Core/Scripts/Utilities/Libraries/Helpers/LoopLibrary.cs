using System;

namespace UnityGameStarter.LoopHelper
{
    public static class LoopLibrary
    {
        public static void Loop2D(int width, int height, Action<int, int> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            LoopND(new[] { width, height }, indices => action(indices[0], indices[1]));
        }

        public static void Loop3D(int width, int height, int depth, Action<int, int, int> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            LoopND(new[] { width, height, depth }, indices => action(indices[0], indices[1], indices[2]));
        }

        public static void LoopND(int[] dimensions, Action<int[]> action)
        {
            if (dimensions == null) throw new ArgumentNullException(nameof(dimensions));
            if (action == null) throw new ArgumentNullException(nameof(action));

            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] < 0)
                    throw new ArgumentOutOfRangeException(nameof(dimensions));
            }

            int[] indices = new int[dimensions.Length];
            LoopRecursive(dimensions, indices, 0, action);
        }

        private static void LoopRecursive(
            int[] dimensions, int[] indices, int dimension, Action<int[]> action)
        {
            if (dimension == dimensions.Length)
            {
                action(indices);
                return;
            }

            for (int i = 0; i < dimensions[dimension]; i++)
            {
                indices[dimension] = i;
                LoopRecursive(dimensions, indices, dimension + 1, action);
            }
        }
    }
}