using UnityEngine;
using TMPro;

public class PriceTag : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private int originalPrice;
    [SerializeField] private GameObject priceText;
    [SerializeField] private ShelfScript shelf;
    void Start()
    {
        priceText.GetComponent<TextMeshProUGUI>().text = "?";
    }

    public void SetPrice(int price, int originalPrice = 0, ShelfScript shelf = null)
    {
        if (shelf != null)
        {
            this.shelf = shelf;
            this.originalPrice = originalPrice;
        }

        Transform childTransform = transform.GetChild(0);

        if (childTransform != null)
        {
            Outline outline = childTransform.GetComponent<Outline>();

            if (outline != null)
            {
                outline.enabled = true;
            }
        }

        this.price = price;

        if (price == 0)
        {
            priceText.GetComponent<TextMeshProUGUI>().text = "?";

            childTransform.GetComponent<Outline>().enabled = false;
        }
        else
        {
            priceText.GetComponent<TextMeshProUGUI>().text = price.ToString() + " TL";
        }
    }

    public void SetVisiblePriceTag()
    {
        if (price == 0)
        {
            return;
        }

        ProductManager.Instance.SetVisiblePriceTag(shelf, originalPrice);
    }

    public int GetPrice()
    {
        return price;
    }
}
