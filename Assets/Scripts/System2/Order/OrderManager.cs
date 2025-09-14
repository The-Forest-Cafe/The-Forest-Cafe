using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    private List<OrderData> currentOrders = new List<OrderData>();

    public delegate void OnOrderAdded(OrderData order);
    public event OnOrderAdded onOrderAdded;

    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    public void AddOrder(OrderData order)
    {
        currentOrders.Add(order);
        onOrderAdded?.Invoke(order);    // UI 쪽에 주문 추가를 알림
    }

    public List<OrderData> GetOrders()
    {
        return currentOrders;
    }
}
