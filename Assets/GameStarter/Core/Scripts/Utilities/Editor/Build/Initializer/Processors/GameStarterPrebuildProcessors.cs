using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace UnityGameStarter.BuildCore.Processors 
{
    internal sealed class GameStarterBeforeBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => -1000;

        public void OnPreprocessBuild(BuildReport report)
        {
            GameStarterBuildBootstrap<BuildContext>.Execute(
                GameStarterBuildPhase.BeforeBuild, new BuildContext(report));
        }
    }

    internal sealed class GameStarterPrepareProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => -900;

        public void OnPreprocessBuild(BuildReport report)
        {
            GameStarterBuildBootstrap<BuildContext>.Execute(
                GameStarterBuildPhase.Prepare, new BuildContext(report));
        }
    }

    internal sealed class GameStarterPreBakeProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => -800;

        public void OnPreprocessBuild(BuildReport report)
        {
            GameStarterBuildBootstrap<BuildContext>.Execute(
                GameStarterBuildPhase.Prebake, new BuildContext(report));
        }
    }

    internal sealed class GameStarterPreValidateProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => -700;

        public void OnPreprocessBuild(BuildReport report)
        {
            GameStarterBuildBootstrap<BuildContext>.Execute(
                GameStarterBuildPhase.Prevalidate, new BuildContext(report));
        }
    }
}