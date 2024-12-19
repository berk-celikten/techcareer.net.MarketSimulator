using UnityEngine;

public class CashierUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI receivedText;
    [SerializeField] private TMPro.TextMeshProUGUI totalText;
    [SerializeField] private TMPro.TextMeshProUGUI chargeText;
    [SerializeField] private TMPro.TextMeshProUGUI givingText;
    private void Start()
    {
        CashierManager.Instance.OnCustomerArrived += HandleCustomerArrived;
        CashierManager.Instance.OnItemScanned += HandleItemScanned;
        CashierManager.Instance.OnTotalUpdated += HandleTotalUpdated;
        CashierManager.Instance.OnTimeToPay += HandleTimeToPay;
        CashierManager.Instance.OnTransactionCompleted += HandleTransactionCompleted;
        CashierManager.Instance.OnCustomerLeft += HandleCustomerLeft;
        CashierManager.Instance.OnChangeUpdated += HandleChangeUpdated;
        CashierManager.Instance.OnReceivedMoneyUpdated += HandleReceivedMoneyUpdated;
    }

    private void OnDestroy()
    {
        if (CashierManager.Instance != null)
        {
            CashierManager.Instance.OnCustomerArrived -= HandleCustomerArrived;
            CashierManager.Instance.OnItemScanned -= HandleItemScanned;
            CashierManager.Instance.OnTotalUpdated -= HandleTotalUpdated;
            CashierManager.Instance.OnTimeToPay -= HandleTimeToPay;
            CashierManager.Instance.OnTransactionCompleted -= HandleTransactionCompleted;
            CashierManager.Instance.OnCustomerLeft -= HandleCustomerLeft;
            CashierManager.Instance.OnChangeUpdated -= HandleChangeUpdated;
            CashierManager.Instance.OnReceivedMoneyUpdated -= HandleReceivedMoneyUpdated;
        }
    }

    private void HandleCustomerArrived()
    {
        if (receivedText != null) receivedText.text = "0 TL";
        if (totalText != null) totalText.text = "0 TL";
        if (chargeText != null) chargeText.text = "0 TL";
        if (givingText != null) givingText.text = "0 TL";
    }

    private void HandleItemScanned(ProductSO product)
    {
        Debug.Log($"Ürün tarandý - {product.productName}, Fiyat: {product.price} TL");
    }

    private void HandleTotalUpdated(decimal total)
    {
        if (totalText != null)
        {
            totalText.text = total + " TL";
        }
    }

    private void HandleTimeToPay()
    {
        if (chargeText != null) chargeText.text = "0 TL";
        if (givingText != null) givingText.text = "0 TL";
    }

    private void HandleTransactionCompleted()
    {
        //Debug.Log("Ödeme tamamlandý");
    }

    private void HandleReceivedMoneyUpdated(decimal receivedMoney)
    {
        if (receivedText != null) receivedText.text = receivedMoney + " TL";
    }

    private void HandleCustomerLeft()
    {
        if (chargeText != null) chargeText.text = "0 TL";
        if (givingText != null) givingText.text = "0 TL";
        if (totalText != null) totalText.text = "0 TL";
        if (receivedText != null) receivedText.text = "0 TL";
    }

    private void HandleChangeUpdated(decimal change, decimal totalChangeToGive, decimal changeGiven)
    {
        if (chargeText != null) chargeText.text = totalChangeToGive + " TL";
        if (givingText != null) givingText.text = changeGiven + " TL";
    }
}
