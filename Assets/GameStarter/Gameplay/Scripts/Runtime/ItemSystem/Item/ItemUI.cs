using UnityEngine;

namespace UnityGameStarter.Gameplay.Item
{
    internal class ItemUI : MonoBehaviour
    {
        public ItemUIData data;

        public void SetUI(ItemData data) 
        {
            SetIcon(data.Icon, data.IconColor, data.IconMaterial);
            SetItemName(data.Name);
            SetItemDescription(data.Description);
        }

        public void Clear() => data = null;

        public void SetIcon(Sprite newIcon, Color newColor, Material newMaterial = null) 
        {
            var icon = data.Icon;

            if (icon == null) return;

            icon.sprite = newIcon;
            icon.color = newColor;
            icon.material = newMaterial;
        }

        public void SetItemName(string newName) 
        {
            var name = data.Name;

            if (name == null) return;

            name.SetText(newName);
        }

        public void SetItemDescription(string newDescription) 
        {
            var description = data.Description;

            if (description == null) return;

            description.SetText(newDescription);
        }
    }
}