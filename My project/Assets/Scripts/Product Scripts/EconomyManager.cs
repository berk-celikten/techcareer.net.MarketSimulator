using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private decimal _currentMoney;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Duplicate EconomyManager instance");
            Destroy(gameObject);
        }
    }

    public decimal CurrentMoney
    {
        get { return _currentMoney; }
        private set
        {
            _currentMoney = value;
            OnMoneyChanged?.Invoke(_currentMoney);
        }
    }

    public event Action<decimal> OnMoneyChanged;

    public decimal AddMoney(decimal amount)
    {
        CurrentMoney += amount;
        return amount;
    }

    public bool TrySpendMoney(decimal amount)
    {
        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            return true;
        }
        return false;
    }
}
