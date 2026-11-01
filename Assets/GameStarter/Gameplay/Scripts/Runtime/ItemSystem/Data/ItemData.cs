using UnityEngine;

namespace UnityGameStarter.Gameplay.Item 
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Unity Game Starter/Gameplay/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        internal GameObject item;

        public string Name => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public GameObject Item => item;
    }
}