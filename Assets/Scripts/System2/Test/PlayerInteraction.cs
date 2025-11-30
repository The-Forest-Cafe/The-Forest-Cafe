using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask npcLayer;

    private void Update()
    {
        // Ray ½Ã°¢È­
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
            CustomerController orderable = hit.collider.GetComponent<CustomerController>();
            if (orderable != null && orderable.isWaitingForOrder)
            {
                ReceiveOrder(orderable);
            }
        }
    }

    private void ReceiveOrder(CustomerController npc)
    {
        OrderData order = new();
        order.customerName = npc.myData.npcName;
        order.drinkName = npc.currentOrder.drinkName;

        npc.OnOrderAccepted();

        OrderManager.Instance.AddOrder(order);
    }
}
