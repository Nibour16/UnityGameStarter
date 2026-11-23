namespace UnityGameStarter.Pool 
{
    /// <summary>
    /// An optional interface for game object if they need to be setup during pool spawn or despawn
    /// </summary>
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}