using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public abstract class BaseGridDebugger : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour sourceBehaviour;

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

        protected IGridDebugSource Source => sourceBehaviour as IGridDebugSource;

        protected void Draw() 
        {
            if (Source == null) return;

            var source = Source;
            var count = source.CellCount;
            float size = source.CellSize;

            if (count.x <= 0 || count.y <= 0 || count.z <= 0 || size <= 0f) return;

            Vector3 start = source.WorldOrigin(false);
            Vector3 right = source.GridRight * size;
            Vector3 up = source.GridUp * size;
            Vector3 forward = source.GridForward * size;

            if (drawBounds)
                DrawBounds(start, right, up, forward, count);

            if (drawGrid)
                DrawGrid(start, right, up, forward, count);

            if (drawOrigin)
                DrawOrigin(size);

            if (drawCellCenters)
                DrawCellCenters(start, right, up, forward, count);
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