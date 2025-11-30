using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public MoneyManager money;

    public float interactRange = 2f;
    public bool hasDrink = false;
    public LayerMask npcLayer;

    private void Update()
    {
        // Ray 시각화
        Debug.DrawRay(transform.position, transform.forward * interactRange, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, npcLayer))
        {
            CustomerController npc = hit.collider.GetComponent<CustomerController>();
            if (npc != null && npc.isWaitingForOrder)
            {
                // 주문 받기
                Debug.Log(npc.name);
                ReceiveOrder(npc);
            }
            if (npc != null && npc.isSitting && hasDrink)
            {
                // 음료 주기
                GiveDrink(npc);
            }
        }
    }

    private void ReceiveOrder(CustomerController npc)
    {
        OrderData order = new();
        order.customerName = npc.myData.npcName;
        order.drinkName = npc.currentOrder.drinkName;

        Debug.Log($"손님({npc.myData.npcName}): 주문 완료");
        npc.OnOrderAccepted();

        OrderManager.Instance.AddOrder(order);
    }

    private void GiveDrink(CustomerController npc)
    {
        // if correct drink
            // money.AddMoney(npc.currentOrder.price);
            // npc 전용 대사 출력
        // else

        OrderData order = new();
        order.customerName = npc.myData.npcName;
        order.drinkName = npc.currentOrder.drinkName;

        OrderManager.Instance.RemoveOrder(order);
    }
}
