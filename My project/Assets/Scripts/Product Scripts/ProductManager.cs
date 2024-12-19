using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro.Examples;
public class ProductManager : MonoBehaviour
{
    public static ProductManager Instance { get; private set; }

    [SerializeField] private List<ProductSO> availableProducts = new List<ProductSO>();
    public Transform spawnArea;
    [SerializeField] private List<LicenseSO> availableLicenses = new List<LicenseSO>();

    [SerializeField] private List<LicenseSO> ownedLicenses = new List<LicenseSO>();

    public GameObject priceTagPrefab;
    public event Action<ProductSO> OnProductPurchased;
    public event Action<ProductSO> OnProductSpawned;
    public event Action<string> OnPurchaseFailed;
    public event Action<LicenseSO> OnLicensePurchased;
    private void Awake()

    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Product Manager Duplicate");
            Destroy(gameObject);
        }
    }
    public void PurchaseProduct(string productName)
    {
        ProductSO product = availableProducts.Find(p => p.productName == productName);

        if (product != null)
        {
            if (EconomyManager.Instance.TrySpendMoney(product.price))
            {
                SpawnProduct(product);
                OnProductPurchased?.Invoke(product);
            }
            else
            {
                OnPurchaseFailed?.Invoke("Yetersiz bakiye");
            }
        }
        else
        {
            OnPurchaseFailed?.Invoke("Ürün bulunamadý");
        }
    }
    private void SpawnProduct(ProductSO product)
    {
        if (spawnArea != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion rotation = Quaternion.Euler(0, 90, 0);
            GameObject box = Instantiate(product.boxPrefab, spawnPosition, rotation, spawnArea);
            Box boxComponent = box.GetComponent<Box>();
            boxComponent.GetComponent<Rigidbody>().isKinematic = true;
            List<ProductSO> productsToInitialize = new List<ProductSO>();
            for (int i = 0; i < 12; i++)
            {
                productsToInitialize.Add(product);
            }
            boxComponent?.SetProductType(product);
            boxComponent?.InitializeProducts(productsToInitialize);
            OnProductSpawned?.Invoke(product);
        }
    }
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPos = spawnArea.position;
        float fixedY = -0.45f;

        bool validPositionFound = false;
        int maxAttempts = 10;
        int attemptCount = 0;

        while (!validPositionFound && attemptCount < maxAttempts)
        {
            spawnPos += new Vector3(
                UnityEngine.Random.Range(-3f, 3f),
                fixedY,
                UnityEngine.Random.Range(-3f, 3f)
            );

            RaycastHit hit;
            if (Physics.Raycast(spawnPos, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Box"))
                {
                    attemptCount++;
                    continue;
                }
            }

            validPositionFound = true;
        }
        if (attemptCount >= maxAttempts)
        {
            Debug.LogWarning("Failed spawn");
        }
        return spawnPos;
    }
    public List<ProductSO> GetAvailableProducts()
    {
        List<ProductSO> licensedProducts = new List<ProductSO>();
        foreach (var product in availableProducts)
        {
            Debug.Log($"Ürün kontrol ediliyor: {product.productName}, Lisans durumu: {CanSellProduct(product)}");
            if (CanSellProduct(product))
            {
                licensedProducts.Add(product);
                Debug.Log($"Satýn alýnabilir ürün: {product.productName}");
            }
        }
        return licensedProducts;
    }

    public ProductSO GetProductByName(string productName)
    {
        return availableProducts.Find(p => p.productName == productName);
    }

    public bool HasLicense(string licenseKeyword)
    {
        return availableProducts.Exists(product => product.licenseKeyword == licenseKeyword);
    }

    public List<LicenseSO> GetAllLicenses()
    {
        return availableLicenses;
    }

    public void AddLicense(string licenseKeyword)
    {
        LicenseSO license = availableLicenses.Find(l => l.licenseKeyword == licenseKeyword);
        if (license != null)
        {
            ownedLicenses.Add(license);
            license.isPurchased = true;
            OnLicensePurchased?.Invoke(license);
        }
    }

    public List<LicenseSO> GetOwnedLicenses()
    {
        List<LicenseSO> licenses = new List<LicenseSO>(ownedLicenses);
        return licenses;
    }

    public bool CanSellProduct(ProductSO product)
    {
        if (string.IsNullOrEmpty(product.licenseKeyword))
        {
            return true;
        }

        foreach (var license in ownedLicenses)
        {
            if (product.licenseKeyword == license.licenseKeyword)
            {
                return true;
            }
        }
        return false;
    }

    public bool PurchaseLicense(string licenseKeyword)
    {
        LicenseSO license = availableLicenses.Find(l => l.licenseKeyword == licenseKeyword);
        if (license != null)
        {
            if (EconomyManager.Instance.TrySpendMoney(license.price))
            {
                AddLicense(licenseKeyword);
                return true;
            }
            else
            {
                OnPurchaseFailed?.Invoke("Yetersiz bakiye");
                return false;
            }
        }
        return false;
    }

    //Fiyatlandýrma 

    [SerializeField] private ShelfScript priceTagShelf;
    public GameObject GetPriceTagPrefab()
    {
        return priceTagPrefab;
    }

    public void SetVisiblePriceTag(ShelfScript shelf = null, int originalPrice = 0)
    {
        if (shelf != null)
        {
            priceTagShelf = shelf;

        }

        priceTagPrefab.SetActive(!priceTagPrefab.activeSelf);

        priceTagPrefab.GetComponent<PriceTagUI>().SetPriceTagShelf(originalPrice);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement fpsMovement = player.GetComponent<PlayerMovement>();
            if (fpsMovement != null)
            {
                fpsMovement.canMove = !priceTagPrefab.activeSelf;
            }
        }

        if (priceTagPrefab.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SetPriceTagShelf(int price)
    {
        priceTagShelf.SetPriceForItemZone(price);
    }
}