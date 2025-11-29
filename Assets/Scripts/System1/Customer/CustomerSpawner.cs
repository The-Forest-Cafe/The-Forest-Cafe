using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    [Header("위치 연결")]
    public Transform spawnPoint;
    public Transform counterPoint;
    public Transform exitPoint;

    [Header("좌석 리스트")]
    public List<Transform> seatPoints;

    // [New] 좌석이 찼는지 확인하는 장부 (true = 누군가 앉음/예약됨)
    private bool[] seatStates;

    [Header("데이터 연결")]
    public List<CustomerData> allCustomers;

    private void Start()
    {
        // 좌석 개수만큼 상태 배열 초기화 (기본값 false: 비어있음)
        seatStates = new bool[seatPoints.Count];

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(5.0f, 10.0f); // 25~40초로 나중에 수정
            yield return new WaitForSeconds(waitTime);

            // [핵심] 빈 자리가 있는지 먼저 확인!
            if (GetAvailableSeatCount() > 0)
            {
                SpawnRandomCustomer();
            }
            else
            {
                Debug.Log("모든 좌석이 꽉 찼습니다! 손님이 오지 않습니다.");
            }
        }
    }

    // 빈 좌석 개수 세기
    private int GetAvailableSeatCount()
    {
        int count = 0;
        for (int i = 0; i < seatStates.Length; i++)
        {
            if (!seatStates[i]) count++; // false인 것만 카운트
        }
        return count;
    }

    private void SpawnRandomCustomer()
    {
        // 1. 빈 좌석들의 번호(인덱스)만 모으기
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < seatStates.Length; i++)
        {
            if (!seatStates[i]) availableIndices.Add(i);
        }

        if (availableIndices.Count == 0) return; // (방어 코드)

        // 2. 빈 좌석 중 하나 랜덤 선택
        int randIdx = Random.Range(0, availableIndices.Count); // 리스트 내의 순번
        int finalSeatIndex = availableIndices[randIdx];        // 실제 좌석 번호

        // [중요] "이 자리는 이제 예약되었습니다" 표시
        seatStates[finalSeatIndex] = true;
        Transform assignedSeat = seatPoints[finalSeatIndex];

        // 3. 랜덤 손님 데이터 선택
        int randomDataIndex = Random.Range(0, allCustomers.Count);
        CustomerData selectedData = allCustomers[randomDataIndex];

        // 4. 생성
        GameObject npcObj = Instantiate(selectedData.npcPrefab, spawnPoint.position, Quaternion.identity);

        CustomerController controller = npcObj.GetComponent<CustomerController>();
        if (controller == null) controller = npcObj.AddComponent<CustomerController>();

        // 5. 초기화 (스포너 자신(this)을 넘겨줘서 나중에 퇴장 보고를 받음)
        controller.Initialize(selectedData, counterPoint, exitPoint, assignedSeat, this);
    }

    // [New] 손님이 나갈 때 호출하는 함수: "저 갑니다, 자리 비우세요"
    public void ReturnSeat(Transform seatTrans)
    {
        // 이 포인트가 몇 번째 좌석이었는지 찾기
        int index = seatPoints.IndexOf(seatTrans);

        if (index != -1)
        {
            seatStates[index] = false; // 다시 빈 자리(false)로 변경
            Debug.Log($"좌석 {index}번이 비워졌습니다. (다음 손님 가능)");
        }
    }
}
