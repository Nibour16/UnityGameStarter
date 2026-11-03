using UnityEngine;
using UnityGameStarter.Events;

namespace UnityGameStarter.Gameplay 
{
    /// <summary>
    /// This MonoBehaviour will destroy its owning gameObject when exit the game.
    /// Usually used for gameObjects that are marked as DontDestoryOnLoad objects
    /// </summary>
    [RequireComponent(typeof(EventListenerRegister))]
    public sealed class GameplayEntity : MonoBehaviour, IAutoEventListener
    {
        [EventListener]
        private void DestroyWhenExitGame(EnterGameExitEvent e) 
        {
            if (e.Reason == ExitGameReason.Exit)
                Destroy(gameObject);
        }
    }
}