using UnityEngine;

public class OrderUI : MonoBehaviour
{
    public Transform orderParent;   // 주문서를 나열할 위치
    public GameObject orderPrefab;  // 주문서 UI 프리팹

    private void Start()
    {
        OrderManager.Instance.onOrderAdded += HandleNewOrder;
    }

    private void HandleNewOrder(OrderData order)
    {
        GameObject newOrderUI = Instantiate(orderPrefab, orderParent);
        newOrderUI.GetComponent<OrderPrefab>().drinkImage.sprite = order.drinkImage;
        newOrderUI.GetComponent<OrderPrefab>().customerImage.sprite = order.customerImage;
    }
}
