using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Dictionary<int, Item> items;
    public int capacity;

    public delegate void InventoryChangedEventHandler();
    public event InventoryChangedEventHandler OnInventoryChanged;

    private void Start()
    {
        items = new Dictionary<int, Item>(capacity);
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= capacity)
        {
            return false;
        }

        items.Add(item.itemID, item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveItem(int itemID)
    {
        items.Remove(itemID);
        OnInventoryChanged?.Invoke();
    }

    public int GetItemQuantity(int itemID)
    {
        if (items.ContainsKey(itemID))
        {
            return items[itemID].quantity;
        }
        return 0;
    }

}
