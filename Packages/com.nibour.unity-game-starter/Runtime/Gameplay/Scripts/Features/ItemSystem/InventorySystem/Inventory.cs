using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityGameStarter.Events;

namespace UnityGameStarter.Gameplay.Item 
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<ItemData> slots;
        [SerializeField] private int maxItemSlots = 10;
        public int MaxItemSlots => maxItemSlots;

        public bool TryAddItem(BaseItem item, out string reason) 
        {
            if (item.Data == null) 
            {
                reason = "Invalid Data";
                return false;
            }

            if (slots.Count >= maxItemSlots) 
            {
                reason = "Inventory is full";
                return false;
            }

            slots.Add(item.Data);

            reason = "Success";
            item.OnAdded();
            EventManager.Instance.Publish(new ItemCollectedEvent(item, reason));
            return true;
        }

        public ItemData GetItemData(string itemName) 
        {
            var data = slots.FirstOrDefault(x => x.Name == itemName);

            if (data == null) 
            {
                Debug.LogError($"AudioManager: Cannot find item {itemName}. ");
                return null;
            }

            return data;
        }

        public ItemData GetItemData(BaseItem item) 
        {
            var data = slots.FirstOrDefault(x => x == item.Data);

            if (data == null)
            {
                Debug.LogError($"AudioManager: Cannot find item {item}. ");
                return null;
            }

            return data;
        }

        public void RemoveItem(BaseItem item) 
        {
            if (item.Data == null) return;

            EventManager.Instance.Publish(new ItemRemovingEvent(item));
            slots.Remove(item.Data);
            item.OnRemoved();
        }
    }
}