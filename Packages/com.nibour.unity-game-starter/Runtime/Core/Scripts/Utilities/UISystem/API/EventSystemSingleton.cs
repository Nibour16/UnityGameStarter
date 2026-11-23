using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.UI 
{
    /// <summary>
    /// Component binded for Unity Event System Object, to make it singleton that prevents duplication
    /// </summary>
    public class EventSystemSingleton : Singleton<EventSystemSingleton>
    {
        [SerializeField] private bool dontDestroyOnLoad = false;
        [SerializeField] private bool disconnectFromParentDuringAwake = true;

        protected override bool DestroyGameObjectOnDuplicate => true;

        protected override void Awake()
        {
            base.Awake();

            if (disconnectFromParentDuringAwake)
                transform.SetParent(null);

            if (dontDestroyOnLoad)
                EnableDontDestroyOnLoad();
        }
    }
}