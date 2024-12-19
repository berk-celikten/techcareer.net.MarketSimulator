using UnityEngine;
using System;
using System.Collections.Generic;

public class CashierManager : MonoBehaviour
{
    public static CashierManager Instance { get; private set; }
    private NPC movingCustomer; // Kasaya doðru hareket eden müþteri

    public Transform checkoutArea;
    [SerializeField] private LayerMask productLayer;

    private List<GameObject> currentCustomerItems = new List<GameObject>();
    private decimal totalCost = 0;
    private decimal receivedMoney = 0;
    private decimal changeToGive = 0;
    private decimal changeGiven = 0;
    private bool isProcessingCustomer = false;
    private bool isProcessingPayment = false;
    private bool isCustomerMoving = false;
    
    private int totalItemsScanned = 0;
    private int totalNPCProcessed = 0;
    private float totalProfit = 0f;

    public AudioClip scanSound;


    public event Action<ProductSO> OnItemScanned;
    public event Action<decimal> OnTotalUpdated;


    public event Action<decimal> OnReceivedMoneyUpdated;
    public event Action OnTimeToPay;
    public event Action<decimal, decimal, decimal> OnChangeUpdated; // (remainingChange, totalChangeToGive, changeGiven)
    public event Action OnTransactionCompleted;
    public event Action OnCustomerArrived;
    public event Action OnCustomerLeft;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (isProcessingCustomer && Input.GetMouseButtonDown(0))
        {
            TryToScanItem();
        }

        if (isProcessingPayment && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Müþteri ödeme iþlemi devam ediyor ve boþluk tuþuna basýldý.");
            CompletePayment();
        }
    }


    private void TryToScanItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, productLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (currentCustomerItems.Contains(hitObject))
            {
                ScanItem(hitObject);
                AudioSource.PlayClipAtPoint(scanSound, Camera.main.transform.position);
            }
        }
    }

    private void ScanItem(GameObject itemObject)
    {
        ProductSO scannedProduct = itemObject.GetComponent<ProductReference>().product;
        if (scannedProduct != null)
        {
            totalCost += itemObject.GetComponent<ProductReference>().shelfPrice;
            totalProfit += itemObject.GetComponent<ProductReference>().shelfPrice - itemObject.GetComponent<ProductReference>().product.price;
            
            OnItemScanned?.Invoke(scannedProduct);
            OnTotalUpdated?.Invoke(totalCost);

            currentCustomerItems.Remove(itemObject);
            Destroy(itemObject);
            if (currentCustomerItems.Count == 0)
            {
                OnTimeToPay?.Invoke();
            }
            totalItemsScanned++;
        }
        
    }
    
    public int TotalItemsScanned
    {
        get { return totalItemsScanned; }
        set { totalItemsScanned = value; }
    }

    public int TotalNPCProcessed
    {
        get { return totalNPCProcessed; }
        set { totalNPCProcessed = value; }
    }
    
    public float TotalProfit
    {
        get { return totalProfit; }
        set { totalProfit = value; }
    }

    public void CustomerArrived(List<ProductReference> items)
    {
        if (isProcessingCustomer)
        {
            Debug.LogWarning("Bir müþteri zaten iþleniyor!");
            return;
        }

        isProcessingCustomer = true;
        isProcessingPayment = false;
        totalCost = 0;
        receivedMoney = 0;
        changeToGive = 0;
        changeGiven = 0;
        OnCustomerArrived?.Invoke();

        foreach (var item in items)
        {
            SpawnItemOnCheckout(item);
        }
        Debug.Log($"{items.Count} items added to checkout for processing.");
    }

    
    public bool CustomerMovingToCashier(NPC customer)
    {
        if (movingCustomer == null || movingCustomer == customer)
        {
            movingCustomer = customer;
            isCustomerMoving = true;
            return true;
        }
        return false;
      
    }
    
    private void SpawnItemOnCheckout(ProductReference item)
    {
        if (checkoutArea != null)
        {
            Vector3 spawnPosition = GetRandomCheckoutPosition();
            GameObject spawnedItem = Instantiate(item.product.prefab, spawnPosition, Quaternion.identity);

            spawnedItem.layer = LayerMask.NameToLayer("cashierLayer");

            //spawnedItem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            ProductReference productRef = spawnedItem.GetComponent<ProductReference>();
            productRef.product = item.product;
            productRef.shelfPrice = item.shelfPrice;

            spawnedItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            currentCustomerItems.Add(spawnedItem);
        }
    }

    private Vector3 GetRandomCheckoutPosition()
    {
        Vector3 spawnPos = checkoutArea.position;
        spawnPos += new Vector3(
          UnityEngine.Random.Range(-0.3f, 0.3f),
          0,
          UnityEngine.Random.Range(-0.3f, 0.3f)
        );
        return spawnPos;
    }

    public void SetReceivedMoney(decimal amount)
    {
        if (isProcessingCustomer && currentCustomerItems.Count == 0)
        {
            receivedMoney = amount;
            changeToGive = receivedMoney - totalCost;
            OnChangeUpdated?.Invoke(changeToGive, changeToGive, changeGiven);
            OnReceivedMoneyUpdated?.Invoke(receivedMoney);
            isProcessingPayment = true;
        }
    }

    public void GiveChange(decimal amount)
    {
        if (changeGiven + amount >= 0)
        {
            changeGiven += amount;
            changeToGive -= amount;
            OnChangeUpdated?.Invoke(changeToGive, receivedMoney - totalCost, changeGiven);
        }
        else
        {
            Debug.Log("Para üstü verilemez");
        }
    }

    private void CompletePayment()
    {
        if (changeToGive == 0m && isProcessingPayment)
        {
            CompleteTransaction();
        }
    }

    private void CompleteTransaction()
    {
        isProcessingPayment = false;

        foreach (GameObject item in currentCustomerItems)
        {
            Destroy(item);
        }

        currentCustomerItems.Clear();
        EconomyManager.Instance.AddMoney(totalCost);
        totalCost = 0;
        receivedMoney = 0;
        changeToGive = 0;
        changeGiven = 0;

        OnTransactionCompleted?.Invoke();
        OnCustomerLeft?.Invoke();
        totalNPCProcessed++;
        //movingCustomer.HandleTransactionCompleted();

        isProcessingCustomer = false;
        isCustomerMoving = false;

        movingCustomer = null;
    }

    public decimal GetCurrentTotal() => totalCost;
    public decimal GetChangeToGive() => changeToGive;
    public decimal GetTotalChangeToGive() => receivedMoney - totalCost;
    public int GetRemainingItemCount() => currentCustomerItems.Count;
    public bool IsProcessingCustomer() => isProcessingCustomer;
    public bool IsCustomerMovingToCashier() => isCustomerMoving;
}
