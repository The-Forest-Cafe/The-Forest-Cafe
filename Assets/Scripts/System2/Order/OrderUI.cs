using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Transform orderParent;   // 주문서를 나열할 위치
    public GameObject orderPrefab;  // 주문서 UI 프리팹

    [SerializeField]
    private List<Sprite> orderSprites = new();
    private Dictionary<string, GameObject> orderPrefabs = new();

    private void Start()
    {
        OrderManager.Instance.onOrderAdded += HandleNewOrder;
        OrderManager.Instance.onOrderRemoved += HandleRemoveOrder;
    }

    private void HandleNewOrder(OrderData order)
    {
        GameObject newOrderUI = Instantiate(orderPrefab, orderParent);
        string drinkName = ChangeDrinkName(order);

        newOrderUI.GetComponent<Image>().sprite = SearchSprite(drinkName);
        orderPrefabs.Add(drinkName, newOrderUI);
    }

    private void HandleRemoveOrder(OrderData order)
    {
        string drinkName = ChangeDrinkName(order);
        orderPrefabs.Remove(drinkName);
    }

    private string ChangeDrinkName(OrderData order)
    {
        switch (order.customerName)
        {
            case "바늘":
                    order.customerName = "Rabbit";
                    break;
            case "이불":
                order.customerName = "Owl";
                break;
            case "포푸":
                order.customerName = "Peng";
                break;
            case "연유":
                order.customerName = "Cat";
                break;
            case "일반 손님1":
                order.customerName = "Otter";
                break;
            case "일반 손님2":
                order.customerName = "Turtle";
                break;
        }

        switch (order.drinkName)
        {
            case "코스믹 블룸":
                order.customerName = "Cosmic";
                break;
            case "유령 슬픔이 라떼":
                order.customerName = "Ghost";
                break;
            case "문어의 꿈 스무디":
                order.customerName = "Octopus";
                break;
            case "심해 진주 버블티":
                order.customerName = "DeapSea";
                break;
            case "별빛 유성차":
                order.customerName = "Star";
                break;
            case "이상한 음료":
                order.customerName = "Otter";
                break;
        }

        return $"{order.customerName}_{order.drinkName}";
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
