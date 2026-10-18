using UnityEngine;
using UnityGameStarter.LoopHelper;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public sealed class GridVisualizer : BaseGridDebugger
    {
        [Header("State Colors")]
        [SerializeField] private Color emptyColor = Color.green;
        [SerializeField] private Color occupiedColor = Color.blue;
        [SerializeField] private Color blockedColor = Color.red;
        [SerializeField] private Color invalidColor = Color.gray;

        [Header("State Display")]
        [SerializeField] private bool drawInvalid = false;

        public void DrawGizmos()
        {
            if (Source == null || !Source.IsGridGenerated) return;
            Draw();
        }

        protected override void DrawBounds(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            foreach (var pair in Source.Grids)
            {
                GridElement[,,] data = pair.Value.GridData;
                if (data == null) continue;

                int sx = data.GetLength(0);
                int sy = data.GetLength(1);
                int sz = data.GetLength(2);

                bool IsValid(int x, int y, int z)
                {
                    if (x < 0 || x >= sx || y < 0 || y >= sy || z < 0 || z >= sz) return false;

                    return data[x, y, z].state != GridState.Invalid;
                }

                Gizmos.color = boundsColor;

                LoopLibrary.Loop3D(sx, sy, sz, (x, y, z) =>
                {
                    if (!IsValid(x, y, z)) return;

                    Vector3 p = start + right * x + up * y + forward * z;

                    if (!IsValid(x - 1, y, z))
                        DrawQuad(p, p + up, p + up + forward, p + forward);

                    if (!IsValid(x + 1, y, z))
                        DrawQuad(p + right, p + right + up,
                            p + right + up + forward, p + right + forward);

                    if (!IsValid(x, y - 1, z))
                        DrawQuad(p, p + right, p + right + forward, p + forward);

                    if (!IsValid(x, y + 1, z))
                        DrawQuad(p + up, p + up + right,
                            p + up + right + forward, p + up + forward);

                    if (!IsValid(x, y, z - 1))
                        DrawQuad(p, p + right, p + right + up, p + up);

                    if (!IsValid(x, y, z + 1))
                        DrawQuad(p + forward, p + right + forward,
                            p + right + up + forward, p + up + forward);
                });
            }
        }

        protected override void DrawCellCenters(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            float radius = Source.CellSize * 0.035f;

            foreach (var pair in Source.Grids)
            {
                GridElement[,,] data = pair.Value.GridData;
                if (data == null) continue;

                int sx = data.GetLength(0);
                int sy = data.GetLength(1);
                int sz = data.GetLength(2);

                LoopLibrary.Loop3D(sx, sy, sz, (x, y, z) =>
                {
                    GridElement element = data[x, y, z];

                    if (!TryGetStateColor(element.state, out Color color)) return;

                    Gizmos.color = color;

                    Vector3 center = start
                        + right * (x + 0.5f)
                        + up * (y + 0.5f)
                        + forward * (z + 0.5f);

                    Gizmos.DrawSphere(center, radius);
                });
            }
        }

        protected override void DrawGrid(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            foreach (var pair in Source.Grids)
            {
                GridElement[,,] data = pair.Value.GridData;
                if (data == null) continue;

                int sx = data.GetLength(0);
                int sy = data.GetLength(1);
                int sz = data.GetLength(2);

                LoopLibrary.Loop3D(sx, sy, sz, (x, y, z) =>
                {
                    GridElement element = data[x, y, z];

                    if (!TryGetStateColor(element.state, out Color color)) return;

                    Gizmos.color = color;

                    Vector3 p = start
                        + right * x
                        + up * y
                        + forward * z;

                    DrawCellEdges(p, right, up, forward);
                });
            }
        }

        protected override void DrawOrigin(float size)
        {
            if (targetGrid == null) return;

            Gizmos.color = originColor;
            Gizmos.DrawSphere(targetGrid.GetOriginWorldPosition(
                Source.OriginType, Source.CenterOriginMode), size * 0.1f);
        }

        private bool TryGetStateColor(GridState state, out Color color)
        {
            switch (state)
            {
                case GridState.Empty:
                    color = emptyColor;
                    return true;

                case GridState.Occupied:
                    color = occupiedColor;
                    return true;

                case GridState.Blocked:
                    color = blockedColor;
                    return true;

                case GridState.Invalid when drawInvalid:
                    color = invalidColor;
                    return true;

                default:
                    color = default;
                    return false;
            }
        }

        private void DrawCellEdges(Vector3 p, Vector3 right, Vector3 up, Vector3 forward)
        {
            Vector3 r = p + right;
            Vector3 u = p + up;
            Vector3 f = p + forward;

            Vector3 ru = p + right + up;
            Vector3 rf = p + right + forward;
            Vector3 uf = p + up + forward;
            Vector3 ruf = p + right + up + forward;

            Gizmos.DrawLine(p, r);
            Gizmos.DrawLine(p, u);
            Gizmos.DrawLine(p, f);

            Gizmos.DrawLine(r, ru);
            Gizmos.DrawLine(r, rf);

            Gizmos.DrawLine(u, ru);
            Gizmos.DrawLine(u, uf);

            Gizmos.DrawLine(f, rf);
            Gizmos.DrawLine(f, uf);

            Gizmos.DrawLine(ru, ruf);
            Gizmos.DrawLine(rf, ruf);
            Gizmos.DrawLine(uf, ruf);
        }

        private void DrawQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, d);
            Gizmos.DrawLine(d, a);
        }
    }
}