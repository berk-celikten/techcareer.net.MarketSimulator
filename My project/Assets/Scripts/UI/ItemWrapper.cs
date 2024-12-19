using UnityEngine;

public class ItemWrapper : MonoBehaviour
{
    public TMPro.TextMeshProUGUI productText;
    public string productName;
    public int productID;
    public int productPrice = 0;

    public void setProductID(int id)
    {
        productID = id;
    }

    public void SetProductName(string newName)
    {
        productName = newName;
        if (productText != null)
        {
            productText.text = productName + " - " + productPrice + " TL";
        }
        else
        {
            Debug.LogError("ProductText bileşeni bulunamadı.");
        }
    }

    public void SetProductPrice(int newPrice)
    {
        productPrice = newPrice;
    }

    public void SpawnProduct()
    {
        UIFunctions uiFunctions = GameObject.Find("Canvas UI(Clone)").GetComponent<UIFunctions>();
        if (uiFunctions != null)
        {
            uiFunctions.OnBuyButtonClicked(productName);
        }
    }
    
}
