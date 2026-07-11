namespace UnityGameStarter.Pool 
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}