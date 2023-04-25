using UnityEngine;

public class PickupItem : Interactable
{
    public Item item;
    public enum PickupType { Health, Hunger, Thirst, Fatigue, Crafting }
    public PickupType pickupType;
    public float pickupValue = 25f;

    public override void Interact()
    {
        // Get the Inventory component from the player and add the item
        Inventory inventory = FindObjectOfType<Inventory>();
        Debug.Log("Found Inventory: " + (inventory != null));

        // Get the NeedsSystem component from the player
        NeedsSystem needsSystem = FindObjectOfType<NeedsSystem>();

        // Check the pickup type and apply the effect
        switch (pickupType)
        {
            case PickupType.Health:
                needsSystem.AddHealth(pickupValue);
                break;
            case PickupType.Hunger:
                needsSystem.AddHunger(pickupValue);
                break;
            case PickupType.Thirst:
                needsSystem.AddThirst(pickupValue);
                break;
            case PickupType.Fatigue:
                needsSystem.AddFatigue(pickupValue);
                break;
            case PickupType.Crafting:
                // Add the item to the inventory
                inventory.AddItem(item);
                break;
        }

        // Implement any other item pickup logic here
        Debug.Log("Item picked up: " + item.itemName);

        // Trigger the OnInteracted event before destroying the GameObject
        TriggerInteracted();

        // Destroy the GameObject after the item is picked up
        Destroy(gameObject);

        // Clear the interaction text after the item is picked up
        InteractionSystem interactionSystem = FindObjectOfType<InteractionSystem>();
        if (interactionSystem != null)
        {
            interactionSystem.ClearInteractionText();
        }
    }
}
