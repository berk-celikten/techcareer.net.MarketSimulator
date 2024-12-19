using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriceTagUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField priceInputField;
    [SerializeField] private Button applyButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI priceText;

    private void Start()
    {
        applyButton.onClick.AddListener(ApplyPrice);
        cancelButton.onClick.AddListener(CancelPriceTag);
    }

    private void ApplyPrice()
    {
        if (int.TryParse(priceInputField.text, out int newPrice))
        {
            if (newPrice > 0)
            {
                ProductManager.Instance.SetPriceTagShelf(newPrice);
                ClosePriceTag();
                priceInputField.text = "";
            }
        }
    }

    private void CancelPriceTag()
    {
        ClosePriceTag();
    }

    private void ClosePriceTag()
    {
        ProductManager.Instance.SetVisiblePriceTag();
    }

    public void SetPriceTagShelf(int price)
    {
        priceText.text = price.ToString() + " TL";
    }
}
