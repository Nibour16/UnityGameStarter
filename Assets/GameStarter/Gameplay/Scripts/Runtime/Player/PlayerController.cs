using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Events;
using UnityGameStarter.PauseManagement;
using UnityGameStarter.UI;

namespace UnityGameStarter.Gameplay.PlayerSystem
{
    public struct OnPlayerPauseEvent { }
    public struct OnPlayerUnpauseEvent { }

    [RequireComponent(typeof(GameplayEntity))]
    [RequireComponent(typeof(EventListenerRegister))]
    public class PlayerController : MonoBehaviour, IAutoEventListener
    {
        [SerializeField] private UIRoot gameplayUIRootPrefab;
        [SerializeField] private string targetPauseMenuName = "PauseMenu";
        [SerializeField] private bool dontDestroyOnLoad = false;

        [Header("UI-less Settings")]
        [SerializeField] private bool pauseGame = true;

        [Header("Advanced")]
        [SerializeField] private bool removeOtherPlayerUI = true;

        private UIRoot _gameplayUIRoot;
        public UIRoot GameplayUIRoot => _gameplayUIRoot;

        private void Awake()
        {
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            InitializeGameplayRoot();
        }

        private void InitializeGameplayRoot() 
        {
            List<UIRoot> roots = new();
            var pendingRoots = FindObjectsByType<UIRoot>();

            foreach (var root in pendingRoots)
            {
                if (!root.CompareTag("PlayerUI")) continue;

                roots.Add(root);
            }

            if (!gameplayUIRootPrefab) 
            {
                Debug.LogError("Root prefab is not defined", this);
                return;
            }

            foreach (var root in roots) 
            {
                if (root.name == gameplayUIRootPrefab.name)
                {
                    _gameplayUIRoot = root;
                }
                else if (removeOtherPlayerUI)
                    Destroy(root);
            }

            if (_gameplayUIRoot == null)
                _gameplayUIRoot = Instantiate(gameplayUIRootPrefab, Vector3.zero, Quaternion.identity);
        }

        public void SetPause()
        {
            if (!GameManager.TryGetInstance(out var instance)) return;

            if (instance.IsPaused) 
            {
                if (UIController.TryGetInstance(out var ui)) 
                {
                    if (_gameplayUIRoot)
                        if (ui.CloseUI(_gameplayUIRoot, targetPauseMenuName)) return;

                    if (ui.CloseUI(targetPauseMenuName)) return;
                }

                if (pauseGame)
                    instance.EnterGame();
                else
                    EventManager.Instance.Publish(new OnPlayerUnpauseEvent());
            }
            else 
            {
                if (UIController.TryGetInstance(out var ui)) 
                {
                    if (_gameplayUIRoot)
                        if (ui.OpenUI(_gameplayUIRoot, targetPauseMenuName)) return;

                    if (ui.OpenUI(targetPauseMenuName)) return;
                }

                if (pauseGame)
                    instance.PauseGame();
                else
                    EventManager.Instance.Publish(new OnPlayerPauseEvent());
            }
        }

        [EventListener]
        private void OnPauseChanged(PauseChangedEvent e)
        {
            if (e.IsPaused) 
            {
                EventManager.Instance.Publish(new OnPlayerPauseEvent());
            }
            else 
            {
                EventManager.Instance.Publish(new OnPlayerUnpauseEvent());
            }
        }
    }
}