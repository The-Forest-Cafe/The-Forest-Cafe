using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
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
            INPCOrderable orderable = hit.collider.GetComponent<INPCOrderable>();
            if (orderable != null && orderable.HasOrder)
            {
                ReceiveOrder(orderable);
            }
        }
    }

    private void ReceiveOrder(INPCOrderable npc)
    {
        OrderData order = npc.GetOrder();
        npc.MarkOrderReceived();

        OrderManager.Instance.AddOrder(order);

        Debug.Log($"주문 받음 : {order.drinkName}");
    }
}
