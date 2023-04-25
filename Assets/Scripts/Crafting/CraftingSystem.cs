using UnityEngine;
using StarterAssets;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    public GameObject housePrefab;
    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    private GameObject currentOutline;
    private MeshRenderer[] currentOutlineRenderers;
    private Camera mainCamera;
    private bool placementMode;
    private bool canPlace;

    private void Start()
    {
        mainCamera = Camera.main;
        StarterAssetsInputs.BuildModePressed += TogglePlacementMode;
        StarterAssetsInputs.InteractPressed += PlaceObject;
        StarterAssetsInputs.ScrollWheelChanged += RotateObject;
    }

    private void OnDestroy()
    {
        StarterAssetsInputs.BuildModePressed -= TogglePlacementMode;
        StarterAssetsInputs.InteractPressed -= PlaceObject;
        StarterAssetsInputs.ScrollWheelChanged -= RotateObject;
    }

    private void Update()
    {
        if (placementMode)
        {
            UpdatePlacement();
            CheckForPlacement();
        }
    }

    private void TogglePlacementMode()
    {
        if (placementMode)
        {
            ExitPlacementMode();
        }
        else
        {
            EnterPlacementMode();
        }
    }

    private void RotateObject(float scrollValue)
    {
        if (placementMode)
        {
            Debug.Log("Scroll Value: " + scrollValue);
            float rotationAngle = scrollValue * rotationSpeed;
            Vector3 currentRotation = currentOutline.transform.eulerAngles;
            currentRotation.y += rotationAngle;
            currentOutline.transform.eulerAngles = currentRotation;
        }
    }



    private void EnterPlacementMode()
    {
        placementMode = true;

        currentOutline = Instantiate(housePrefab);
        currentOutline.name = "Preview Object";
        currentOutlineRenderers = currentOutline.GetComponentsInChildren<MeshRenderer>();
        SetOutlineMaterials(validPlacementMaterial);

        foreach (Collider col in currentOutline.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    private void ExitPlacementMode()
    {
        placementMode = false;

        if (currentOutline != null)
        {
            Destroy(currentOutline);
        }
    }

    private void UpdatePlacement()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, groundLayerMask))
        {
            currentOutline.SetActive(true);
            currentOutline.transform.position = hit.point;

            // Combine surface alignment and Y-axis rotation
            Quaternion surfaceAlignment = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Quaternion yRotation = Quaternion.Euler(0, currentOutline.transform.eulerAngles.y, 0);
            currentOutline.transform.rotation = surfaceAlignment * yRotation;
        }
        else
        {
            currentOutline.SetActive(false);
        }
    }


    private void CheckForPlacement()
    {
        if (currentOutline.activeInHierarchy)
        {
            Collider[] colliders = Physics.OverlapBox(currentOutline.transform.position, currentOutline.GetComponentInChildren<Renderer>().bounds.extents, currentOutline.transform.rotation, obstacleLayerMask);

            if (colliders.Length > 0)
            {
                canPlace = false;
                SetOutlineMaterials(invalidPlacementMaterial);
            }
            else
            {
                canPlace = true;
                SetOutlineMaterials(validPlacementMaterial);
            }
        }
        else
        {
            canPlace = false;
        }

    }

    private void PlaceObject()
    {
        if (placementMode && canPlace)
        {
            Debug.Log("Placing");
            GameObject newHouse = Instantiate(housePrefab, currentOutline.transform.position, currentOutline.transform.rotation);
            ExitPlacementMode();
        }
    }

    private void SetOutlineMaterials(Material material)
    {
        foreach (var renderer in currentOutlineRenderers)
        {
            renderer.material = material;
        }
    }
}
