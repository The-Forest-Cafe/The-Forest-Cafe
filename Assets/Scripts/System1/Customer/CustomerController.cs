using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerController : MonoBehaviour
{
    [Header("상태 확인용")]
    public bool isWaitingForOrder = false;
    public bool isSitting = false;
    public CustomerData myData;

    [Header("현재 주문")]
    public Recipe currentOrder;

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

        MoveToCounter();
    }


    private void Update()
    {
        if (anim != null && agent != null && agent.enabled)
        {
            bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
            anim.SetBool("IsWalking", isMoving);
        }
    }

    private void OnMouseDown()
    {
        if (isWaitingForOrder)
        {
            OnOrderAccepted();
        }
    }

    public void OnOrderAccepted()
    {
        isWaitingForOrder = false;
        StopAllCoroutines();

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
        Debug.Log($"손님({myData.npcName}): \"{currentOrder.drinkName}\" 주문");
        StartCoroutine(PatienceTimer());
    }

    private void DecideOrder()
    {
        if (myData.possibleMenus != null && myData.possibleMenus.Count > 0)
        {
            int randomIndex = Random.Range(0, myData.possibleMenus.Count);
            currentOrder = myData.possibleMenus[randomIndex];
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

        Debug.Log($"({myData.npcName}): 착석 완료");
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

        if (isAngry) Debug.Log($"({myData.npcName}): 대기 시간이 지나 퇴장");
        else Debug.Log($"({myData.npcName}) 퇴장");

        agent.SetDestination(exitPoint.position);
        Destroy(gameObject, 5.0f);
    }
}
