using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;
using UnityGameStarter.CommonData;

namespace UnityGameStarter.BuildCore 
{
    public abstract class GameStarterBuildContext : GameStarterContext
    {
        public BuildReport Report { get; }

        public GameStarterBuildContext(BuildReport report)
        {
            Report = report;
        }
    }

    public class BuildContext : GameStarterBuildContext
    {
        public BuildContext(BuildReport report) : base(report) { }
    }

    public class SceneBuildContext : GameStarterBuildContext
    {
        public Scene Scene { get; }

        public SceneBuildContext(BuildReport report, Scene scene) : base(report)
        {
            Scene = scene;
        }
    }
}