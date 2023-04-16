using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject panel;
    public GameObject inventorySlotPrefab;
    public Transform inventorySlotContainer;

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
        bool isInventoryOpen = panel.activeSelf;
        if (isInventoryOpen)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
            UpdateInventoryUI();
        }

    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in inventorySlotContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate the UI with items from the inventory
        foreach (KeyValuePair<Item, int> entry in inventory.items)
        {
            Item item = entry.Key;
            int itemQuantity = entry.Value;

            // Instantiate the inventory slot prefab
            GameObject slot = Instantiate(inventorySlotPrefab, inventorySlotContainer);

            // Set the item icon and quantity text
            Image icon = slot.GetComponentInChildren<Image>();
            TextMeshProUGUI quantityText = slot.GetComponentInChildren<TextMeshProUGUI>();
            icon.sprite = item.icon;
            quantityText.text = itemQuantity.ToString();
        }
    }
}
