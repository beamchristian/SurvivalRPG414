using UnityEngine;

public class PickupItem : Interactable
{
    public Item item;

    public override void Interact()
    {
        // Get the Inventory component from the player and add the item
        Inventory inventory = FindObjectOfType<Inventory>();
        Debug.Log("Found Inventory: " + (inventory != null));

        // Add the item to the inventory
        inventory.AddItem(item);

        // Implement any other item pickup logic here
        Debug.Log("Item picked up: " + item.itemName);

        // Destroy the GameObject after the item is picked up
        Destroy(gameObject);
    }
}

