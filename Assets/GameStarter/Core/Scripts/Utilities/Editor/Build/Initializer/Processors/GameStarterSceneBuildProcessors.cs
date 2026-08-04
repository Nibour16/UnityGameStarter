using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace UnityGameStarter.BuildCore.Processors 
{
    internal sealed class GameStarterSceneBuildProcessor : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            GameStarterBuildBootstrap<SceneBuildContext>.Execute(
                GameStarterBuildPhase.BuildScene, new SceneBuildContext(report, scene));
        }
    }
}