using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Dictionary<Item, int> items;
    public int capacity;

    public delegate void InventoryChangedEventHandler();
    public event InventoryChangedEventHandler OnInventoryChanged;

    private void Start()
    {
        items = new Dictionary<Item, int>(capacity);
    }

    public bool AddItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        else if (items.Count >= capacity)
        {
            return false;
        }
        else
        {
            items.Add(item, 1);
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            if (items[item] > 1)
            {
                items[item]--;
            }
            else
            {
                items.Remove(item);
            }

            OnInventoryChanged?.Invoke();
        }
    }

    public int GetItemQuantity(Item item)
    {
        if (items.ContainsKey(item))
        {
            return items[item];
        }
        return 0;
    }
}
