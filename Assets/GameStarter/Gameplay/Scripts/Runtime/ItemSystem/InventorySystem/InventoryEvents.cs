namespace UnityGameStarter.Gameplay.Item 
{
    public abstract class ItemCollectionEvent
    {
        public readonly BaseItem item;
        
        public ItemCollectionEvent(BaseItem item)
        {
            this.item = item;
        }
    }

    public class ItemCollectedEvent : ItemCollectionEvent
    {
        public readonly string log;
        public ItemCollectedEvent(BaseItem item, string log = "") : base(item) 
        {
            this.log = log;
        }
    }

    public class ItemRemovingEvent : ItemCollectionEvent
    {
        public ItemRemovingEvent(BaseItem item) : base(item) { }
    }
}