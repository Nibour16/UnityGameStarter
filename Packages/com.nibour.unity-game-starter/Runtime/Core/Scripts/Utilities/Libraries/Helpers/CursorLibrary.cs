using UnityEngine;

namespace UnityGameStarter.CursorStatics
{
    /// <summary>
    /// Library that manages mouse cursor state
    /// </summary>
    public static class CursorLibrary
    {
        private static Object _cilent;
        public static Object Cilent => _cilent;

        /// <summary>
        /// Define the client that can control the cursor
        /// </summary>
        public static void SetClient(Object cilent) => _cilent = cilent;

        /// <summary>
        /// Set cursor by checking the the client
        /// </summary>
        public static void SetCursor(this Object cilent, CursorLockMode lockMode, bool visible)
        {
            if (_cilent == null)
                SetClient(cilent);

            else if (_cilent != cilent) return;

            SetCursor(lockMode, visible);
        }

        /// <summary>
        /// Force set the cursor without checking the client
        /// </summary>
        public static void SetCursor(CursorLockMode lockMode, bool visible)
        {
            Cursor.lockState = lockMode;
            Cursor.visible = visible;
        }

        /// <summary>
        /// Lock the cursor by checking the client
        /// </summary>
        public static void Lock(this Object cilent)
        {
            cilent.SetCursor(CursorLockMode.Locked, false);
        }

        /// <summary>
        /// Force lock the cursor without checking the client
        /// </summary>
        public static void ForceLock() 
        {
            SetCursor(CursorLockMode.Locked, false);
        }

        /// <summary>
        /// Unlock the cursor by checking the client
        /// </summary>
        public static void Unlock(this Object cilent)
        {
            cilent.SetCursor(CursorLockMode.None, true);
        }

        /// <summary>
        /// Force unlock the cursor without checking the client
        /// </summary>
        public static void ForceUnlock() 
        {
            SetCursor(CursorLockMode.None, true);
        }

        /// <summary>
        /// Confine the cursor by checking the client
        /// </summary>
        public static void Confine(this Object cilent)
        {
            cilent.SetCursor(CursorLockMode.Confined, true);
        }

        /// <summary>
        /// Force confine the cursor without checking the client
        /// </summary>
        public static void Confine()
        {
            SetCursor(CursorLockMode.Confined, true);
        }
    }
}

