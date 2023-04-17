using UnityEngine;
using System;

public abstract class Interactable : MonoBehaviour
{
    public string itemName;
    public event Action OnInteracted;

    public void TriggerInteracted()
    {
        OnInteracted?.Invoke();
    }

    public abstract void Interact();
}
