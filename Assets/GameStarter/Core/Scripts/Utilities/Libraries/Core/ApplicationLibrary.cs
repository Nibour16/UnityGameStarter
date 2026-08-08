namespace UnityGameStarter.ApplicationStatics
{
    public static class ApplicationLibrary
    {
        public static void QuitApp() 
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}