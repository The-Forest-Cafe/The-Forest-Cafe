using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public MoneyManager money;

    // 주문 받기/음료 전달
    public float interactRange = 2f;
    public bool hasDrink = false;
    public LayerMask npcLayer;

    // 음료 제작/구매창 패널 관리
    public Camera mapCamera;
    public LayerMask targetLayers;
    public GameObject makeDrinkPanel;
    public GameObject purchasePanel;

    // 음료 모델링
    [SerializeField]
    private List<GameObject> drinks = new();
    private GameObject currentModeling = null;

    private void Update()
    {
        // Ray 시각화
        Debug.DrawRay(transform.position, transform.forward * interactRange, Color.red);

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
            // QuestManager.Instance.UpdateQuestProgress(npc, drink);
        }
        else
        {
            ScoreManager.Instance.ApplyPenalty(PenaltyType.WrongServing, 5);
        }

        OrderData order = new();
        order.customerName = npc.myData.npcName;
        order.drinkName = npc.currentOrder.drinkName;

        OrderManager.Instance.RemoveOrder(order);
        RemoveDrinkModel();
    }

    public void SetDrinkModel()
    {
        // 플레이어 손에 모델링 생성
        foreach (var drink in drinks)
        {
            Debug.Log(DrinkMakingManager.Instance.currentDrink.recipeID);
            Debug.Log(drink.name);

            if (DrinkMakingManager.Instance.currentDrink.recipeID == drink.name)
            {
                drink.SetActive(true);
                currentModeling = drink;
            }
        }
    }

    public void RemoveDrinkModel()
    {
        if (currentModeling)
        {
            hasDrink = false;
            currentModeling.SetActive(false);
        }
    }
}
