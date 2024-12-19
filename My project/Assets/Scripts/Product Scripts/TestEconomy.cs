using UnityEngine;

public class TestEconomy : MonoBehaviour
{

    void Start()
    {
        EconomyManager.Instance.AddMoney(100);
    }
}
