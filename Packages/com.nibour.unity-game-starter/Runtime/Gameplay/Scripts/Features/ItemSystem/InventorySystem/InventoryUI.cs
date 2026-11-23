using System;

using UnityEngine;
using UnityGameStarter.Events;
using UnityGameStarter.Gameplay.UI.Menu;

namespace UnityGameStarter.Gameplay.Item 
{
    [RequireComponent(typeof(EventListenerRegister))]
    public class InventoryUI : PauseMenuBehaviour, IAutoEventListener
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private ItemUI uiPrefab;
        [SerializeField] private Transform content;

        private ItemUI[] _slots;

        public override void Init()
        {
            base.Init();

            _slots = new ItemUI[inventory.MaxItemSlots];

            var trans = content? content : transform;

            var currentSlots = trans.GetComponentsInChildren<ItemUI>();

            if (currentSlots.Length > _slots.Length)
            {
                for (int i = _slots.Length; i < currentSlots.Length; i++)
                {
                    Destroy(currentSlots[i].gameObject);
                }

                Array.Resize(ref currentSlots, _slots.Length);
            }

            for (int i = 0; i < _slots.Length; i++) 
            {
                if (_slots[i] != null) continue;

                if (i < 0 || i >= _slots.Length)
                    throw new IndexOutOfRangeException($"Invalid Slot: {i}, Slot length is: {_slots.Length}");

                var slot = i < currentSlots.Length ? currentSlots[i] : null;

                if (slot == null) 
                {
                    if (uiPrefab == null)
                    {
                        Debug.LogError("ItemUI Prefab is not assigned.", this);
                        continue;
                    }

                    slot = Instantiate(uiPrefab, trans, false);
                }

                _slots[i] = slot;
            }
        }

        [EventListener]
        private void OnItemCollected(ItemCollectedEvent e) 
        {
            var data = e.item.Data;

            foreach (var slot in _slots) 
            {
                if (slot == null) 
                {
                    slot.SetUI(data);
                    break;
                }
            }
        }

        [EventListener]
        private void OnItemRemoved(ItemRemovingEvent e) 
        {
            foreach(var slot in _slots) 
            {
                if (slot.data.Name.text == e.item.name)
                    slot.Clear();
            }
        }
    }
}