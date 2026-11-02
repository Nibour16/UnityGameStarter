using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGameStarter.Gameplay.Item 
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Unity Game Starter/Gameplay/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic Properties")]
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        [Header("Advanced")]
        [SerializeField] private Color iconColor = Color.white;
        [SerializeField] private Material iconMaterial;

        internal GameObject item;

        public string Name => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public Color IconColor => iconColor;
        public Material IconMaterial => iconMaterial;

        public GameObject Item => item;
    }

    [System.Serializable]
    public class ItemUIData
    {
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image icon;

        public TMP_Text Name => itemName;
        public TMP_Text Description => description;
        public Image Icon => icon;
    }
}