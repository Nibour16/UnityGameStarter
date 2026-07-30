using UnityEngine;
using UnityGameStarter.ServiceLocatorPattern;
using UnityGameStarter.ServiceLocatorPattern.FacadeModule;

namespace UnityGameStarter.InputSystem 
{
    /// <summary>
    /// An example of Input Facade, for reference only. 
    /// </summary>
    class SampleInputFacade : BaseSingletonFacade<SampleInputFacade, IService, InputManager>, IService
    {
        // TODO: Add service properties to pass player input values

        protected override void Awake()
        {
            base.Awake();

            // Strongly recommended to enable don't destroy on load if the manager is DDoL singleton
            EnableDontDestroyOnLoad();  
        }
    }
}

