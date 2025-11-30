using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class CustomerController : MonoBehaviour
{
    [Header("상태 확인용")]
    public bool isWaitingForOrder = false;
    public bool isSitting = false;
    public CustomerData myData;

    [Header("현재 주문")]
    public Recipe currentOrder;

    [Header("말풍선")]
    public GameObject bubbleExclamation; // 느낌표
    public GameObject bubbleHappy;       // 좋음
    public GameObject bubbleAngry;       // 화남

    private NavMeshAgent agent;
    private Animator anim;
    private Transform counterPoint;
    private Transform exitPoint;
    private Transform mySeatPoint;
    private CustomerSpawner mySpawner;

    private float patienceTime = 20.0f;

    public void Initialize(CustomerData data, Transform counter, Transform exit, Transform seat, CustomerSpawner spawner)
    {
        myData = data;
        counterPoint = counter;
        exitPoint = exit;
        mySeatPoint = seat;
        mySpawner = spawner;

        agent = GetComponent<NavMeshAgent>();
        if (agent == null) agent = gameObject.AddComponent<NavMeshAgent>();

        // 애니메이터
        anim = GetComponentInChildren<Animator>();

        if (anim != null)
        {
            anim.applyRootMotion = false;
        }

        // 초기화
        HideAllBubbles();

        MoveToCounter();
    }


    private void Update()
    {
        if (anim != null && agent != null && agent.enabled)
        {
            bool isMoving = agent.velocity.sqrMagnitude > 0.01f;
            anim.SetBool("IsWalking", isMoving);
        }
    }

    private void HideAllBubbles()
    {
        if (bubbleExclamation != null) bubbleExclamation.SetActive(false);
        if (bubbleHappy != null) bubbleHappy.SetActive(false);
        if (bubbleAngry != null) bubbleAngry.SetActive(false);
    }

    private IEnumerator ShowTimedBubble(GameObject bubble, float duration)
    {
        HideAllBubbles(); 
        if (bubble != null)
        {
            bubble.SetActive(true);
            yield return new WaitForSeconds(duration);
            bubble.SetActive(false);
        }
    }

    public void OnOrderAccepted()
    {
        isWaitingForOrder = false;
        StopAllCoroutines();

        if (bubbleExclamation != null) bubbleExclamation.SetActive(false);

        Debug.Log($"손님({myData.npcName}): 주문 완료");
        MoveToSeat();
    }

    private void MoveToCounter()
    {
        agent.SetDestination(counterPoint.position);
        StartCoroutine(CheckArrivalRoutine(true));
    }

    private void MoveToSeat()
    {
        if (mySeatPoint != null)
        {
            agent.SetDestination(mySeatPoint.position);
            StartCoroutine(CheckArrivalRoutine(false));
        }
    }

    private IEnumerator CheckArrivalRoutine(bool isToCounter)
    {
        while (true)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {

                if (anim != null) anim.SetBool("IsWalking", false);

                if (isToCounter) StartOrderWait();
                else SitDown();
                yield break;
            }
            yield return null;
        }
    }

    private void StartOrderWait()
    {
        isWaitingForOrder = true;
        DecideOrder();

        if (currentOrder != null)
        {
            HideAllBubbles();
            if (bubbleExclamation != null) bubbleExclamation.SetActive(true);

            Debug.Log($"손님({myData.npcName}): \"{currentOrder.drinkName}\" 주문");
            StartCoroutine(PatienceTimer());
        }
        else
        {
            Debug.Log($"손님({myData.npcName}): (주문할 수 있는 메뉴가 없음)");
            LeaveCafe(false);
        }
    }

    // ID 비교
    private void DecideOrder()
    {
        if (myData.possibleMenus != null && myData.possibleMenus.Count > 0)
        {
            List<Recipe> unlockedCandidates = new List<Recipe>();

            if (RecipeManager.Instance != null)
            {
                var unlockedDataList = RecipeManager.Instance.GetUnlockedRecipes();

                foreach (var menu in myData.possibleMenus)
                {
                    // ID 비교
                    bool isUnlocked = unlockedDataList.Exists(r => r.recipeId == menu.recipeID);

                    if (isUnlocked)
                    {
                        unlockedCandidates.Add(menu);
                    }
                }
            }
            else
            {
                unlockedCandidates.AddRange(myData.possibleMenus);
            }

            if (unlockedCandidates.Count > 0)
            {
                int randomIndex = Random.Range(0, unlockedCandidates.Count);
                currentOrder = unlockedCandidates[randomIndex];
            }
            else
            {
                Debug.LogWarning($"[Customer] {myData.npcName}: 해금된 메뉴가 없음");
                currentOrder = null;
            }
        }
        else
        {
            currentOrder = null;
        }
    }

    private void SitDown()
    {
        isSitting = true;
        agent.enabled = false;

        if (anim != null)
        {
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsSitting", true);
        }

        if (mySeatPoint.childCount > 0)
        {
            Transform realSitPos = mySeatPoint.GetChild(0);
            transform.position = realSitPos.position;
            transform.rotation = realSitPos.rotation;
        }
        else
        {
            transform.rotation = mySeatPoint.rotation;
        }

        Debug.Log($"({myData.npcName}): 착석 완료. 음료 대기 시작(20초)");

        //음료가 나오는 타이머 시작
        StartCoroutine(ServingPatienceTimer());
    }

    //음료 대기 타이머
    private IEnumerator ServingPatienceTimer()
    {
        float timer = patienceTime; // 20초
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        // 시간 초과
        Debug.Log($"손님({myData.npcName}): 음료가 너무 늦어요");
        LeaveCafe(true);
    }

    //음료 서빙 (성공 시 호출)
    public void OnDrinkServed()
    {
        StopAllCoroutines();

        // 서빙 성공
        StartCoroutine(ShowTimedBubble(bubbleHappy, 4.0f));

        StartCoroutine(HappyWaitTimer());
    }

    //음료 마시는 시간
    private IEnumerator HappyWaitTimer()
    {
        Debug.Log($"손님({myData.npcName}): 음료 받음 (20초 후 퇴장)");
        yield return new WaitForSeconds(20.0f);
        LeaveCafe(false); // 만족하며 퇴장
    }

    private IEnumerator PatienceTimer()
    {
        float timer = patienceTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        LeaveCafe(true);
    }

    public void LeaveCafe(bool isAngry)
    {
        isWaitingForOrder = false;
        isSitting = false;

        StopAllCoroutines();

        if (anim != null)
        {
            anim.SetBool("IsSitting", false);
        }

        if (!agent.enabled)
        {
            if (mySeatPoint != null) transform.position = mySeatPoint.position;
            agent.enabled = true;
        }

        if (mySpawner != null && mySeatPoint != null)
        {
            mySpawner.ReturnSeat(mySeatPoint);
        }

        if (isAngry)
        {
            OrderData order = new();
            order.customerName = myData.npcName;
            order.drinkName = currentOrder.drinkName;
            OrderManager.Instance.RemoveOrder(order);

            ScoreManager.Instance.ApplyPenalty(PenaltyType.CustomerLeave, 10);

            Debug.Log($"({myData.npcName}): 대기 시간이 지나 퇴장");
            StartCoroutine(ShowTimedBubble(bubbleAngry, 3.0f));
        }
        else
        {
            Debug.Log($"({myData.npcName}) 퇴장");
            HideAllBubbles();
        }

        agent.SetDestination(exitPoint.position);
        Destroy(gameObject, 3.0f);
    }
}