using UnityEngine;

[CreateAssetMenu(fileName = "GameplayData", menuName = "Scriptable Objects/Unity Game Starter/Gameplay Data")]
public class GameplayData : ScriptableObject
{
    [Min(0f)] public float gameTimeScale = 1f;

    public readonly float initialGameTimeScale = 1f;

    public GameplayData() 
    {
        initialGameTimeScale = gameTimeScale;
    }
}
