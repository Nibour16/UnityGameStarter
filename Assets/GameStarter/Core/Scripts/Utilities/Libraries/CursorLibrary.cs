using UnityEngine;

namespace UnityGameStarter.CursorStatics
{
    public static class CursorLibrary
    {
        public static void SetCursor(CursorLockMode lockMode, bool visible)
        {
            Cursor.lockState = lockMode;
            Cursor.visible = visible;
        }

        public static void Lock()
        {
            SetCursor(CursorLockMode.Locked, false);
        }

        public static void Unlock()
        {
            SetCursor(CursorLockMode.None, true);
        }

        public static void Confine()
        {
            SetCursor(CursorLockMode.Confined, true);
        }
    }
}

