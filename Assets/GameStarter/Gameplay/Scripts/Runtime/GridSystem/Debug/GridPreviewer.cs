using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public sealed class GridPreviewer : BaseGridDebugger
    {
        private void OnDrawGizmos() => Draw();

        protected override void DrawGrid(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            Gizmos.color = gridColor;
            
            for (int y = 0; y <= count.y; y++)
            {
                for (int z = 0; z <= count.z; z++)
                {
                    Vector3 a = start + up * y + forward * z;
                    Gizmos.DrawLine(a, a + right * count.x);
                }
            }

            for (int x = 0; x <= count.x; x++)
            {
                for (int z = 0; z <= count.z; z++)
                {
                    Vector3 a = start + right * x + forward * z;
                    Gizmos.DrawLine(a, a + up * count.y);
                }
            }

            for (int x = 0; x <= count.x; x++)
            {
                for (int y = 0; y <= count.y; y++)
                {
                    Vector3 a = start + right * x + up * y;
                    Gizmos.DrawLine(a, a + forward * count.z);
                }
            }
        }

        protected override void DrawBounds(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            Gizmos.color = boundsColor;

            Vector3 r = right * count.x;
            Vector3 u = up * count.y;
            Vector3 f = forward * count.z;

            Vector3 p000 = start;
            Vector3 p100 = start + r;
            Vector3 p010 = start + u;
            Vector3 p110 = start + r + u;

            Vector3 p001 = start + f;
            Vector3 p101 = start + r + f;
            Vector3 p011 = start + u + f;
            Vector3 p111 = start + r + u + f;

            DrawQuad(p000, p100, p110, p010);
            DrawQuad(p001, p101, p111, p011);

            Gizmos.DrawLine(p000, p001);
            Gizmos.DrawLine(p100, p101);
            Gizmos.DrawLine(p010, p011);
            Gizmos.DrawLine(p110, p111);
        }

        private void DrawQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, d);
            Gizmos.DrawLine(d, a);
        }

        protected override void DrawOrigin(float size)
        {
            Gizmos.color = originColor;
            Gizmos.DrawSphere(Source.WorldOrigin(true), size * 0.1f);
        }

        protected override void DrawCellCenters(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            Gizmos.color = centersColor;

            float radius = Source.CellSize * 0.035f;

            for (int x = 0; x < count.x; x++)
            {
                for (int y = 0; y < count.y; y++)
                {
                    for (int z = 0; z < count.z; z++)
                    {
                        Vector3 center = start
                            + right * (x + 0.5f)
                            + up * (y + 0.5f)
                            + forward * (z + 0.5f);

                        Gizmos.DrawSphere(center, radius);
                    }
                }
            }
        }

        [ContextMenu("Log Grid Info")]
        private void LogGridInfo()
        {
            if (Source == null)
            {
                Debug.LogWarning("GridDebugger: Source is missing or does not implement IGridDebugSource.",
                    this);
                return;
            }

            var source = Source;

            Debug.Log($"Grid Debug Info\n" +
                $"CellCount: {source.CellCount}\n" +
                $"CellSize: {source.CellSize}\n" +
                $"GridSize: {(Vector3)source.CellCount * source.CellSize}\n" +
                $"Origin: {source.Origin(true)}\n" +
                $"WorldOrigin: {source.WorldOrigin(true)}", this);
        }
    }
}