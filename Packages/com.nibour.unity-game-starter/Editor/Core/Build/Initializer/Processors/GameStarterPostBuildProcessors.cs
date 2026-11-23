using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace UnityGameStarter.BuildCore.Processors 
{
    internal sealed class GameStarterPostBakeProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 800;

        public void OnPostprocessBuild(BuildReport report)
        {
            GameStarterBuildBootstrap<BuildContext>.Execute(
                GameStarterBuildPhase.PostBake, new BuildContext(report));
        }
    }

    internal sealed class GameStarterPostValidateProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 900;

        public void OnPostprocessBuild(BuildReport report)
        {
            GameStarterBuildBootstrap<BuildContext>.Execute(
                GameStarterBuildPhase.PostValidate, new BuildContext(report));
        }
    }

    internal sealed class GameStarterPostBuildProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 1000;

        public void OnPostprocessBuild(BuildReport report)
        {
            GameStarterBuildBootstrap<BuildContext>.Execute(
                GameStarterBuildPhase.AfterBuild, new BuildContext(report));
        }
    }
}