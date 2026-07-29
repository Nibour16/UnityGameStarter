using UnityEngine;

namespace UnityGameStarter.UnityObjectStatics 
{
    public static class ObjectLibrary
    {
        public static bool IsValid(this object obj) 
        {
            if (obj is Object unityObj)
                return unityObj != null;

            return obj != null;
        }

        public static bool IsDestroyed(this Object obj)
            => !ReferenceEquals(obj, null) && obj == null;
    }
}