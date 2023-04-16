using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject panel;
    public TextMeshProUGUI uiText;

    private void Start()
    {
        uiText = GetComponentInChildren<TextMeshProUGUI>();

        if (uiText != null)
        {
            uiText.text = "";
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in children of InventoryUI GameObject.");
        }
    }

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

        uiText.text = "";


        // Populate the UI with items from the inventory
        foreach (KeyValuePair<Item, int> entry in inventory.items)
            {
                Item item = entry.Key;
                int itemQuantity = entry.Value;

                // Add the item information to the UI text
                uiText.text += $"{item.itemName}: {itemQuantity}\n";
                Debug.Log(itemQuantity);
                Debug.Log(item.itemName);
            }
        

    }
}
