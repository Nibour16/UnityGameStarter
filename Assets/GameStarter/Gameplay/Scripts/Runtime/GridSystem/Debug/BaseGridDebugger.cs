using System;
using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public abstract class BaseGridDebugger : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour sourceBehaviour;
        [SerializeField] private string targetLayerName = "Default";

        [Header("Display")]
        [SerializeField] private bool drawGrid = true;
        [SerializeField] private bool drawBounds = true;
        [SerializeField] private bool drawOrigin = true;
        [SerializeField] private bool drawCellCenters = false;

        [Header("Colors")]
        [SerializeField] protected Color gridColor = Color.green;
        [SerializeField] protected Color boundsColor = Color.yellow;
        [SerializeField] protected Color originColor = Color.red;
        [SerializeField] protected Color centersColor = Color.cyan;

        public IGridDebugSource Source => sourceBehaviour as IGridDebugSource;

        protected Layer targetLayer;
        protected Grid targetGrid;

        protected void Draw() 
        {
            if (Source == null) return;

            if (!TryGetDebugLayer(out targetLayer)) return;

            if (!TryGetDebugGrid(out targetGrid)) return;

            var source = Source;
            var count = source.GetCellCount(targetLayer);
            float size = source.CellSize;

            if (count.x <= 0 || count.y <= 0 || count.z <= 0 || size <= 0f) return;

            Vector3 right = source.GetGridRight(targetLayer) * size;
            Vector3 up = source.GetGridUp(targetLayer) * size;
            Vector3 forward = source.GetGridForward(targetLayer) * size;

            Vector3 firstCellCenter = targetGrid.GetCellWorldPosition(
                Vector3Int.zero, out _, GridOriginType.LowerLeftCorner);

            Vector3 start = firstCellCenter
                - (right + up + forward) * 0.5f;

            if (drawBounds)
                DrawBounds(start, right, up, forward, count);

            if (drawGrid)
                DrawGrid(start, right, up, forward, count);

            if (drawOrigin)
                DrawOrigin(size);

            if (drawCellCenters)
                DrawCellCenters(start, right, up, forward, count);
        }

        private bool TryGetDebugLayer(out Layer layer) 
        {
            layer = default;

            if (Source == null) return false;

            layer = Source.GetLayerByName(targetLayerName);

            return true;
        }

        private bool TryGetDebugGrid(out Grid grid)
        {
            grid = null;

            if (Source == null) return false;

            grid = Source.GetGridByLayer(targetLayerName);

            return grid != null;
        }

        protected abstract void DrawGrid(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count);

        protected abstract void DrawBounds(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count);

        protected abstract void DrawOrigin(float size);

        protected abstract void DrawCellCenters(
            Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count);
    }
}