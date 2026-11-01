namespace UnityGameStarter.Gameplay.Item 
{
    public abstract class ItemCollectionEvent
    {
        private readonly BaseItem _item;
        public BaseItem Item => _item;

        public ItemCollectionEvent(BaseItem item)
        {
            _item = item;
        }
    }

    public class ItemCollectedEvent : ItemCollectionEvent
    {
        public ItemCollectedEvent(BaseItem item) : base(item) { }
    }

    public class ItemRemovingEvent : ItemCollectionEvent
    {
        public ItemRemovingEvent(BaseItem item) : base(item) { }
    }
}