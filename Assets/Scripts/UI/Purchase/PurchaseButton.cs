using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    TotalSum totalSum;
    int price = 0;

    void Start()
    {
        totalSum = Object.FindAnyObjectByType<TotalSum>();
    }

    public void OnPurchase()
    {
        price = totalSum.totalSum;
        MoneyManager.Instance.SpendMoney(price);
    }
}
