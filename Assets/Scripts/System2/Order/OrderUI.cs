using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Transform orderParent;   // 주문서를 나열할 위치
    public GameObject orderPrefab;  // 주문서 UI 프리팹

    [SerializeField]
    private List<Sprite> orderSprites = new();

    private void Start()
    {
        OrderManager.Instance.onOrderAdded += HandleNewOrder;
    }

    private void HandleNewOrder(OrderData order)
    {
        GameObject newOrderUI = Instantiate(orderPrefab, orderParent);
        Debug.Log($"{order.customerName}_{order.drinkName}");
        newOrderUI.GetComponent<Image>().sprite = SearchSprite($"{order.customerName}_{order.drinkName}");
    }

    private Sprite SearchSprite(string spriteName)
    {
        foreach (Sprite sprite in orderSprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }

        return null;
    }
}
