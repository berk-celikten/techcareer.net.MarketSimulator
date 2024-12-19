using UnityEngine;
using UnityEngine.UI;

public class RaycastHelper : MonoBehaviour
{
    public float rayDistance = 5f;
    public Camera playerCamera;
    public LayerMask itemLayer;
    public Text itemNameText;

    private GameObject lastHoveredItem;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, itemLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != lastHoveredItem)
            {
                if (lastHoveredItem != null)
                {
                    SetHoverEffect(lastHoveredItem, false);
                }

                SetHoverEffect(hitObject, true);

                lastHoveredItem = hitObject;
            }
        }
        else
        {
            if (lastHoveredItem != null)
            {
                SetHoverEffect(lastHoveredItem, false);
                lastHoveredItem = null;
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
    }

    void SetHoverEffect(GameObject item, bool isHovered)
    {
        Outline outline = item.GetComponent<Outline>();
        if (outline != null)
        {
            ApplyHoverEffect(outline, isHovered);
        }
    }

    void ApplyHoverEffect(Outline outline, bool isHovered)
    {
        if (isHovered)
        {
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.green;
            outline.OutlineWidth = 5f;
        }
        else
        {
            if (outline != null)
            {
                outline.OutlineWidth = 0f;
            }
        }
    }
}