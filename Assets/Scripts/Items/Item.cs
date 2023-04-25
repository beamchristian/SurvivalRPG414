using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Default,
        Crafting
    }

    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public GameObject prefab;
}
