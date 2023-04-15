using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this namespace

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemPrefab;
    public GameObject panel;

    private void OnEnable()
    {
        inventory.OnInventoryChanged += UpdateInventoryUI;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= UpdateInventoryUI;
    }

    public void ToggleInventory()
    {
        panel.SetActive(!panel.activeSelf);
    }

    private void UpdateInventoryUI()
    {
        // Clear previous UI elements
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate the UI with items from the inventory
        foreach (KeyValuePair<int, Item> entry in inventory.items)
        {
            int itemID = entry.Key;
            Item item = entry.Value;

            string itemName = item.itemName;
            int itemQuantity = inventory.GetItemQuantity(itemID);

            // Add the item information to the UI text
            uiText.text += $"{itemName}: {itemQuantity}\n";
        }

    }
}
