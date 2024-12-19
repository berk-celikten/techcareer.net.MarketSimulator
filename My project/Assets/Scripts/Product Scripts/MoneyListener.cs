using UnityEngine;
using TMPro;

public class MoneyListener : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
        if (moneyText == null)
        {
            Debug.LogError("TextMeshProUGUI bileþeni bulunamadý!");
            return;
        }

        EconomyManager.Instance.OnMoneyChanged += UpdateMoneyText;
        UpdateMoneyText(EconomyManager.Instance.CurrentMoney);
    }

    void OnDestroy()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnMoneyChanged -= UpdateMoneyText;
        }
    }

    private void UpdateMoneyText(decimal amount)
    {
        if (moneyText != null)
        {
            moneyText.text = amount.ToString("N2") + " TL";
        }
    }
}
