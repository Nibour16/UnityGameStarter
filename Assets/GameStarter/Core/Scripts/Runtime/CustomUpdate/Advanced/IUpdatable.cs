namespace UnityGameStarter.CustomUpdate.Advanced 
{
    public interface IUpdatable
    {
        void CustomUpdate(float deltaTime);
    }

    public interface IFixedUpdatable
    {
        void CustomFixedUpdate(float deltaTime);
    }

    public interface ILateUpdatable
    {
        void CustomLateUpdate(float deltaTime);
    }
}