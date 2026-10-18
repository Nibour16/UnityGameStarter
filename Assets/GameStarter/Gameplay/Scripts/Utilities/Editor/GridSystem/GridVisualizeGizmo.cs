using UnityEditor;

namespace UnityGameStarter.Gameplay.GridSystem.Editor 
{
    public static class GridVisualizeGizmo
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGridVisualizer(
            GridVisualizer visualizer, GizmoType gizmoType)
        {
            if (visualizer == null || visualizer.Source == null || !visualizer.Source.IsGridGenerated)
                return;

            visualizer.DrawGizmos();
        }
    }
}