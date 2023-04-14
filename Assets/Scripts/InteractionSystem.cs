using UnityEngine;
using StarterAssets;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    public float interactionDistance = 2.0f;
    public TextMeshProUGUI interactionText;
    private Camera _mainCamera;
    private Interactable _currentInteractable;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateCurrentInteractable();
    }

    private void OnEnable()
    {
        StarterAssetsInputs.InteractPressed += HandleInteraction;
    }

    private void OnDisable()
    {
        StarterAssetsInputs.InteractPressed -= HandleInteraction;
    }

    private void UpdateCurrentInteractable()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        int interactableLayerMask = LayerMask.GetMask("Interactable");

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayerMask))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != _currentInteractable)
            {
                _currentInteractable = interactable;
                interactionText.text = _currentInteractable ? _currentInteractable.name + " [E]" : "";
                interactionText.enabled = _currentInteractable != null;
            }
        }
        else if (_currentInteractable != null)
        {
            _currentInteractable = null;
            interactionText.text = "";
            interactionText.enabled = false;
        }
    }

    private void HandleInteraction()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }
}
