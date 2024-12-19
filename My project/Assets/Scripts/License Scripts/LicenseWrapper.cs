using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LicenseWrapper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button buyButton;

    public LicenseSO license;

    public void SetLicenseInfo(LicenseSO license)
    {
        this.license = license;
        headerText.text = license.licenseName;
        descriptionText.text = license.description;
    }

    public void OnBuyButtonClicked()
    {
        if (ProductManager.Instance.PurchaseLicense(license.licenseKeyword))
        {
            buyButton.interactable = false;
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Bought";
        }
        else
        {
            Debug.Log("Yetersiz bakiye");
        }
    }
}

