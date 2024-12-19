using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;

public class DayChanger : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI soldItemText;
    public TextMeshProUGUI clientText;
    public TextMeshProUGUI profitText;
    public GameObject endDayUI;
    public Button nextDayButton;
    
    private int totalItemsScanned = 0;
    private int totalNPCProcessed = 0;
    private int totalDayCount = 1;
    private float totalProfitGain = 0f;

    void OnEnable()
    {
        SaveSystem.Instance.OnDayEnd += HandleDayEnd;
    }
    
    void OnDisable()
    {
        SaveSystem.Instance.OnDayEnd -= HandleDayEnd;
    }

    void HandleDayEnd(string text)
    {
        endDayUI.SetActive(true);
        nextDayButton.onClick.AddListener(SaveSystem.Instance.NextDay);

        totalItemsScanned = CashierManager.Instance.TotalItemsScanned;
        totalNPCProcessed = CashierManager.Instance.TotalNPCProcessed;
        totalDayCount = SaveSystem.Instance.DayCount;
        totalProfitGain = CashierManager.Instance.TotalProfit;
        
        
        soldItemText.text = $"{totalItemsScanned}";
        clientText.text = $"{totalNPCProcessed}";
        dayText.text = $"Day {totalDayCount}";
        profitText.text = $"{totalProfitGain}";
        
    }
}
