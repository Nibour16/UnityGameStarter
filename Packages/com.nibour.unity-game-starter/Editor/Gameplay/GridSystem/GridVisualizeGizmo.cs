using UnityEditor;
using UnityGameStarter.Gameplay.GridSystem;

namespace UnityGameStarter.EditorUtilities.Gameplay.GridSystem 
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