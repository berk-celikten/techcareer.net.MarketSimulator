using UnityEngine;
using System.Collections.Generic;

public class ShelfScript : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPlace;
    [SerializeField] private float minGap = 0.02f;
    [SerializeField] private int maxObjects = 0;
    [SerializeField] private GameObject priceLabel;

    private Vector3 prefabSize;
    private List<Vector3> availableLocalPositions = new List<Vector3>();
    private bool isInitialized = false;
    private int currentItemCount = 0;


    public bool isThisShelf = false;
    [SerializeField] public int shelfPrice = 0;

    public bool PlaceObject()
    {
        if (prefabToPlace == null)
        {
            Debug.LogError("Yerle�tirilecek prefab ata.");
            return false;
        }

        if (!isInitialized)
        {
            InitializePositions();
        }

        return PlaceSingleObject();
    }

    private void InitializePositions()
    {
        prefabSize = prefabToPlace.GetComponent<Renderer>().bounds.size;
        CalculateOptimalLayoutAndPositions();
        isInitialized = true;
    }

    private bool PlaceSingleObject()
    {
        if (availableLocalPositions.Count > 0)
        {
            Vector3 localPosition = availableLocalPositions[0];
            availableLocalPositions.RemoveAt(0);

            GameObject newObject = Instantiate(prefabToPlace, transform);
            newObject.name = $"PlacedObject_{currentItemCount}";
            newObject.transform.localPosition = localPosition;
            //newObject.transform.localRotation = Quaternion.Euler(90f, 0f, 45f);

            Rigidbody rb = newObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            currentItemCount++;
            return true;
        }
        else
        {
            Debug.Log("Yerle�tirilecek daha fazla pozisyon yok.");
            return false;
        }
    }

    void CalculateOptimalLayoutAndPositions()
    {
        availableLocalPositions.Clear();

        Vector3 shelfSize = GetComponent<BoxCollider>().size;
        Vector3 shelfLocalCenter = Vector3.zero;
        Vector3 shelfLocalBottomCenter = shelfLocalCenter - new Vector3(0, 0, shelfSize.z / 2); // Z ekseni kullan�l�yor

        float availableWidth = shelfSize.x;
        float availableHeight = shelfSize.y; // Derinlik yerine y�kseklik kullan�l�yor

        // Raf�n en-boy oran�n� hesapla
        float shelfAspectRatio = availableWidth / availableHeight;

        // En iyi s�tun ve sat�r say�s�n� hesapla
        int idealColumns = Mathf.RoundToInt(Mathf.Sqrt(maxObjects * shelfAspectRatio));
        int idealRows = Mathf.CeilToInt((float)maxObjects / idealColumns);

        // Raf i�inde s�tun ve sat�rlar� ayarla
        int maxColumns = Mathf.Min(idealColumns, Mathf.FloorToInt(availableWidth / prefabSize.x));
        int maxRows = Mathf.Min(idealRows, Mathf.FloorToInt(availableHeight / prefabSize.y)); // Z yerine Y kullan�l�yor

        // maxObjects'� a�mad���m�zdan emin olmak i�in yeniden hesapla
        int totalObjects = Mathf.Min(maxColumns * maxRows, maxObjects);
        maxRows = Mathf.CeilToInt((float)totalObjects / maxColumns);

        // Bo�luklar� hesapla
        float totalWidth = maxColumns * prefabSize.x;
        float columnGap = (availableWidth - totalWidth) / (maxColumns + 1);

        float totalHeight = maxRows * prefabSize.y; // Z yerine Y kullan�l�yor
        float rowGap = (availableHeight - totalHeight) / (maxRows + 1);

        // Minimum bo�lu�u sa�la
        columnGap = Mathf.Max(columnGap, minGap);
        rowGap = Mathf.Max(rowGap, minGap);

        float startX = shelfLocalBottomCenter.x - (totalWidth + (maxColumns - 1) * columnGap) / 2 + prefabSize.x / 2;
        float startY = shelfLocalBottomCenter.y - (totalHeight + (maxRows - 1) * rowGap) / 2 + prefabSize.y / 2; // Z yerine Y kullan�l�yor

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                if (availableLocalPositions.Count >= maxObjects) break;

                Vector3 localPosition = new Vector3(
                    startX + col * (prefabSize.x + columnGap),
                    startY + row * (prefabSize.y + rowGap), // Y ekseni kullan�l�yor
                    shelfLocalBottomCenter.z + (prefabSize.z / 2 + 0.055f) // Z ekseni sabit , 055f kadar yukar� kayd�r�ld�
                );

                availableLocalPositions.Add(localPosition);
            }
            if (availableLocalPositions.Count >= maxObjects) break;
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);

        foreach (Vector3 localPos in availableLocalPositions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(localPos), 0.1f);
        }
    }

    public bool PutItem(GameObject item)
    {
        if (prefabToPlace != null && prefabToPlace != item.GetComponent<ProductReference>().product.prefab)
        {
            return false; // Ayn� prefabdan gelmiyorsa n
        }
        prefabToPlace = item.GetComponent<ProductReference>().product.prefab;
        maxObjects = item.GetComponent<ProductReference>().product.maxCountOnShelf;

        bool placed = PlaceObject();

        if (placed)
        {
            SetPriceForItemZone(item.GetComponent<ProductReference>().product.price);
        }

        return placed;
    }

    public void SetPriceForItemZone(int price)
    {
        shelfPrice = price;
        UpdatePriceLabel(shelfPrice);
    }

    private void UpdatePriceLabel(int price)
    {
        if (priceLabel != null)
        {
            priceLabel.GetComponent<PriceTag>().SetPrice(price, prefabToPlace.GetComponent<ProductReference>().product.price, this);
        }
    }

    public bool TakeItem(ProductSO item)
    {
        if (currentItemCount == 0)
        {
            Debug.LogWarning("Rafta al�nacak �r�n yok.");
            return false;
        }

        if (item.prefab != prefabToPlace)
        {
            Debug.LogWarning("Bu nesne bu raf i�in uygun de�il.");
            return false;
        }

        Transform lastChild = transform.GetChild(currentItemCount - 1);
        availableLocalPositions.Add(lastChild.localPosition);

        currentItemCount--; // Nesne al�nd���nda sayac� azalt

        if (currentItemCount == 0)
        {
            shelfPrice = 0;
            UpdatePriceLabel(shelfPrice);
            isInitialized = false;
            prefabToPlace = null;
            maxObjects = 0;
        }

        Destroy(lastChild.gameObject);

        return true;
    }

    public int GetItemCount()
    {
        return currentItemCount; // Mevcut nesne say�s�n� d�nd�r
    }

    public ProductReference TakeLastItemForNPC()
    {
        if (currentItemCount == 0)
        {
            return null;
        }

        Transform lastChild = transform.GetChild(currentItemCount - 1);
        availableLocalPositions.Add(lastChild.localPosition);

        ProductReference productRef = lastChild.GetComponent<ProductReference>();

        if (productRef != null && productRef.product != null)
        {
            productRef.shelfPrice = shelfPrice;
            ProductSO takenItem = productRef.product;

            currentItemCount--; // Nesne al�nd���nda sayac� azalt

            if (currentItemCount == 0)
            {
                shelfPrice = 0;
                UpdatePriceLabel(0);
                isInitialized = false;
                prefabToPlace = null;
                maxObjects = 0;
            }

            Destroy(lastChild.gameObject);

            return productRef;
        }

        return null;
    }
}
