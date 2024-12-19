using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;

    private void Start()
    {
        EconomyManager.Instance.OnMoneyChanged += UpdateMoneyText;
        UpdateMoneyText(EconomyManager.Instance.CurrentMoney);
    }

    private void OnDestroy()
    {
        EconomyManager.Instance.OnMoneyChanged -= UpdateMoneyText;
    }

    private void UpdateMoneyText(decimal amount)
    {
        moneyText.text = $"{amount:N2} TL";
    }
}
