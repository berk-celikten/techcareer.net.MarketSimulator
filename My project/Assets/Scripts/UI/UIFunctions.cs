using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class UIFunctions : MonoBehaviour
{
    public GameObject[] desktopMenus;
    public GameObject supplyMenuPrefab;
    public GameObject productPrefab;

    public GameObject itemWrapperPrefab;

    public GameObject licenseWrapperPrefab;
    public GameObject licensePrefab;

    private void Start()
    {
        ProductManager.Instance.OnProductPurchased += HandleProductPurchased;
        ProductManager.Instance.OnPurchaseFailed += HandlePurchaseFailed;
        ProductManager.Instance.OnProductSpawned += HandleProductSpawned;
        ProductManager.Instance.OnLicensePurchased += HandleLicensePurchased;

        displayProduct();
        displayLicenses();
    }

    private void displayProduct()
    {
        List<ProductSO> products = ProductManager.Instance.GetAvailableProducts();
        foreach (ProductSO product in products)
        {
            //Debug.Log(product.productName);
            GameObject itemWrapper = Instantiate(itemWrapperPrefab, supplyMenuPrefab.transform);
            ItemWrapper itemWrapperComponent = itemWrapper.GetComponent<ItemWrapper>();
            if (itemWrapperComponent != null)
            {
                itemWrapperComponent.SetProductPrice(product.price);
                itemWrapperComponent.SetProductName(product.productName);
            }
            else
            {
                Debug.LogError("ItemWrapper bileşeni bulunamadı.");
            }
        }
    }

    private void HandleLicensePurchased(LicenseSO license)
    {
        foreach (Transform child in supplyMenuPrefab.transform)
        {
            Destroy(child.gameObject);
        }
        displayProduct();
    }

    private void displayLicenses()
    {
        List<LicenseSO> licenses = ProductManager.Instance.GetAllLicenses();
        foreach (LicenseSO license in licenses)
        {
            GameObject itemWrapper = Instantiate(licensePrefab, licenseWrapperPrefab.transform);
            LicenseWrapper licenseWrapper = itemWrapper.GetComponent<LicenseWrapper>();
            if (licenseWrapper != null)
            {
                licenseWrapper.SetLicenseInfo(license);
            }
            else
            {
                Debug.LogError("LicenseWrapper bileşeni bulunamadı.");

            }
        }
    }

    private void buyLicense(string licenseKeyword)
    {
        ProductManager.Instance.AddLicense(licenseKeyword);
    }

    public void OnBuyButtonClicked(string productName)
    {
        ProductManager.Instance.PurchaseProduct(productName);
    }

    private void HandleProductPurchased(ProductSO product)
    {
        Debug.Log($"{product.productName} satın alındı");
        // UI'ı güncelle, ses çal, vb.
    }

    private void HandlePurchaseFailed(string reason)
    {
        Debug.Log($"Satın alma başarısız: {reason}");
        // UI'da hata mesajını göster
    }

    private void HandleProductSpawned(ProductSO product)
    {
        //Debug.Log($"{product.productName} oluşturuldu");
        // Gerekirse kamera odakla, efekt oynat, vb.
    }

    private void OnDestroy()
    {
        // Event aboneliklerini kaldır
        ProductManager.Instance.OnProductPurchased -= HandleProductPurchased;
        ProductManager.Instance.OnPurchaseFailed -= HandlePurchaseFailed;
        ProductManager.Instance.OnProductSpawned -= HandleProductSpawned;
    }

    public void returnDesktop()
    {
        for (int i = 1; i < desktopMenus.Length; i++)
        {
            desktopMenus[i].SetActive(false);
        }
        desktopMenus[0].SetActive(true);
    }

    public void openMenu(int index)
    {
        for (int i = 0; i < desktopMenus.Length; i++)
        {
            desktopMenus[i].SetActive(i == index);
        }
    }
}
