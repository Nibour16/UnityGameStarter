using System;

namespace UnityGameStarter.CommonData 
{
    public abstract class BaseInitializer<T> where T : Enum 
    {
        public readonly T phase;
        public readonly int order;

        public BaseInitializer(T phase, int order)
        {
            this.phase = phase;
            this.order = order;
        }
    }

    public class Initializer<T> : BaseInitializer<T> where T : Enum
    {
        public Action action;
        
        public Initializer(T phase, int order, Action action) : base(phase, order)
        {
            this.action = action;
        }
    }

    public class Initializer<T, TContext> : BaseInitializer<T> 
        where T : Enum where TContext : GameStarterContext
    {
        public Action<TContext> action;

        public Initializer(T phase, int order, Action<TContext> action) : base(phase, order)
        {
            this.action = action;
        }
    }

    public abstract class GameStarterContext { }
}