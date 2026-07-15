using UnityEngine;
using UnityGameStarter.SingletonPattern;
using UnityGameStarter.SingletonPattern.RuntimeSingletonBootstrap;

namespace UnityGameStarter.Gameplay.Core
{
    [RuntimeSingleton(-290)]
    public class GameManager : Singleton<GameManager>
    {
        
    }
}