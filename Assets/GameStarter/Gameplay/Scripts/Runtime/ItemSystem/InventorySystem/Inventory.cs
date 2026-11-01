using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityGameStarter.Events;

namespace UnityGameStarter.Gameplay.Item 
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<ItemData> inventory;
        [SerializeField] private int maxItemSlot = 10;

        public void AddItem(BaseItem item) 
        {
            if (item.Data == null) return;

            if (inventory.Count >= maxItemSlot) return;

            inventory.Add(item.Data);

            item.OnAdded();
            EventManager.Instance.Publish(new ItemCollectedEvent(item));
        }

        public ItemData GetItemData(string itemName) 
        {
            var data = inventory.FirstOrDefault(x => x.Name == itemName);

            if (data == null) 
            {
                Debug.LogError($"AudioManager: Cannot find item {itemName}. ");
                return null;
            }

            return data;
        }

        public ItemData GetItemData(BaseItem item) 
        {
            var data = inventory.FirstOrDefault(x => x == item.Data);

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
            inventory.Remove(item.Data);
            item.OnRemoved();
        }
    }
}