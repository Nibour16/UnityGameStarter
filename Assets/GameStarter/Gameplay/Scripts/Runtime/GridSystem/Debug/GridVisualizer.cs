using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public sealed class GridVisualizer : BaseGridDebugger
    {
        protected override void DrawBounds(Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            throw new System.NotImplementedException();
        }

        protected override void DrawCellCenters(Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            throw new System.NotImplementedException();
        }

        protected override void DrawGrid(Vector3 start, Vector3 right, Vector3 up, Vector3 forward, Vector3Int count)
        {
            throw new System.NotImplementedException();
        }

        protected override void DrawOrigin(float size)
        {
            throw new System.NotImplementedException();
        }
    }
}