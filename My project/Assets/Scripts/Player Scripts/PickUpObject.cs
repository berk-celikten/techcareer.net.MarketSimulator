using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public AudioClip dropClip; // Býrakma sesini buraya atayýn


    public Transform holdPosition;
    public float pickUpRange = 5f;
    public LayerMask pickUpLayer;
    public float maxTiltAngle = 45f;
    public float shelfPlaceRange = 10f;


    private GameObject heldObject;
    public GameObject trashcan;


     void Start()
    {
        trashcan = GameObject.FindGameObjectWithTag("Trashcan");
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                TryPlaceOnShelf();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && heldObject != null && heldObject.CompareTag("Box"))
        {
            TryTakeItemFromShelf();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (heldObject != null)
            {
                DropObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.C) && heldObject != null && heldObject.CompareTag("Box"))
        {
            heldObject.GetComponent<Box>().ToggleBox();
        }

        if (heldObject != null && heldObject.CompareTag("Box"))
        {
            AdjustBoxRotation();
        }
        if (Input.GetKeyDown(KeyCode.E) && heldObject != null && trashcan != null)
        {
            TryDeleteBoxInTrashcan();
        }
    }

    private void TryPickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpRange, pickUpLayer))
        {
            if (hit.transform.CompareTag("Box"))
            {
                heldObject = hit.transform.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.SetParent(holdPosition);
                heldObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
                heldObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Ölçeði 0.5 olarak ayarla
            }
            else if (hit.transform.CompareTag("priceTag"))
            {
                hit.transform.parent.GetComponent<PriceTag>().SetVisiblePriceTag();
            }
        }
    }

    private void TryPlaceOnShelf()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("pickUpLayer"); //Box Layerý

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shelfPlaceRange, layerMask))
        {
            if (hit.transform.CompareTag("Shelf"))
            {
                ShelfScript shelfTest = hit.transform.GetComponent<ShelfScript>();
                if (shelfTest != null)
                {
                    heldObject.GetComponent<Box>().PlaceProductsOnShelf(shelfTest);
                }
            }
        }
    }

    private void TryTakeItemFromShelf()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shelfPlaceRange))
        {
            if (hit.transform.CompareTag("Shelf"))
            {
                ShelfScript shelfTest = hit.transform.GetComponent<ShelfScript>();
                if (shelfTest != null)
                {
                    heldObject.GetComponent<Box>().TakeItemFromShelf(shelfTest);
                }
            }
        }
    }

    private void DropObject()
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.SetParent(null);


        // Býrakma sesini çal
        if (dropClip != null)
        {
            AudioSource.PlayClipAtPoint(dropClip, transform.position);
        }


        heldObject = null;
    }

    private void AdjustBoxRotation()
    {
        float pitch = Camera.main.transform.localEulerAngles.x;
        if (pitch > 180) pitch -= 360;

        float tilt = Mathf.Clamp(pitch, -maxTiltAngle, maxTiltAngle);

        heldObject.transform.localRotation = Quaternion.Euler(0, 90, tilt);
    }

    private void TryDeleteBoxInTrashcan()
    {
        float distanceToTrashcan = Vector3.Distance(heldObject.transform.position, trashcan.transform.position);
        if (distanceToTrashcan <= 2f)
        {
            Destroy(heldObject);
            Debug.Log("Box removed from the scene.");
            heldObject = null;
        }
    }
}
