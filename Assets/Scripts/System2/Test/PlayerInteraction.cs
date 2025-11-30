using System.Collections;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public MoneyManager money;

    public float interactRange = 2f;
    public bool hasDrink = false;
    public LayerMask npcLayer;

    public Camera mapCamera;
    public LayerMask targetLayers;
    public GameObject makeDrinkPanel;
    public GameObject purchasePanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition); 

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayers))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("클릭된 오브젝트: " + clickedObject.name);
                if (clickedObject.layer == LayerMask.NameToLayer("Computer")) { 
                    purchasePanel.SetActive(true); 
                } 
                else if (clickedObject.layer == LayerMask.NameToLayer("Coffee Machine")) { 
                    makeDrinkPanel.SetActive(true); 
                } 
            } 
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
                GiveDrink(npc, DrinkMakingManager.Instance.currentDrink);
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

    private void GiveDrink(CustomerController npc, Recipe drink)
    {
        if (npc.currentOrder == drink)
        {
            npc.OnDrinkServed();
            money.AddMoney(npc.currentOrder.price);
            QuestManager.Instance.UpdateQuestProgress(npc, drink);
        }

        OrderData order = new();
        order.customerName = npc.myData.npcName;
        order.drinkName = npc.currentOrder.drinkName;

        OrderManager.Instance.RemoveOrder(order);
    }
}
