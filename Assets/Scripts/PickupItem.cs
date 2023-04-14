using UnityEngine;

public class PickupItem : Interactable
{
    public override void Interact()
    {
        // Implement your item pickup logic here.
        Debug.Log("Item picked up!");
    }
}
