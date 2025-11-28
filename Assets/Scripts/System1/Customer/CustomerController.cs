using UnityEngine;
using UnityEngine.AI; // NavMeshAgent 사용을 위해 필수
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
    private Animator anim; // [New] 애니메이터 컴포넌트
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

        // [New] 애니메이터 가져오기 (자식 오브젝트에 있을 수도 있으니InChildren 사용)
        anim = GetComponentInChildren<Animator>();

        MoveToCounter();
    }

    // [New] 매 프레임마다 걷는지 체크해서 애니메이션 재생
    private void Update()
    {
        if (anim != null && agent != null && agent.enabled)
        {
            // 움직이는 속도가 0.1보다 크면 '걷는 중'으로 판단
            // sqrMagnitude는 속도의 제곱(성능 최적화용)
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

        Debug.Log($"손님({myData.npcName}): 주문 감사합니다! ({currentOrder.drinkName}) 자리에 가서 기다릴게요.");
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
                // 도착하면 걷기 애니메이션 끄기
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
        Debug.Log($"손님({myData.npcName}): (카운터 도착) 여기요~ \"{currentOrder.drinkName}\" 주세요! (20초 대기)");
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

        // [New] 앉는 애니메이션 켜기
        if (anim != null)
        {
            anim.SetBool("IsWalking", false); // 혹시 켜져있을까봐 끄기
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

        Debug.Log($"손님({myData.npcName}): (착석 완료) 음료 기다리는 중...");
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

        // [New] 앉기 애니메이션 끄기 (일어나기)
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

        if (isAngry) Debug.Log($"손님({myData.npcName}): 너무 늦어! (퇴장)");
        else Debug.Log($"손님({myData.npcName}): 잘 가요~ (퇴장)");

        agent.SetDestination(exitPoint.position);
        Destroy(gameObject, 5.0f);
    }
}
