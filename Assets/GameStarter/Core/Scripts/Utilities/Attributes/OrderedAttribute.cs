using System;

namespace UnityGameStarter.StarterAttributes 
{
    public abstract class OrderedAttribute : Attribute
    {
        public int Order { get; }

        protected OrderedAttribute(int order = 0)
        {
            Order = order;
        }
    }
}