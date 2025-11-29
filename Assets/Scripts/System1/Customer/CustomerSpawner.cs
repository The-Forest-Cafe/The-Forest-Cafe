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

    //좌석이 찼는지 확인
    private bool[] seatStates;

    [Header("데이터 연결")]
    public List<CustomerData> allCustomers;

    private void Start()
    {
        // 좌석 개수만큼 상태 배열 초기화
        seatStates = new bool[seatPoints.Count];

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(3.0f, 7.0f); // 25~40초로 나중에 수정
            yield return new WaitForSeconds(waitTime);

     
            if (GetAvailableSeatCount() > 0)
            {
                SpawnRandomCustomer();
            }
            else
            {
                Debug.Log("모든 좌석 착석 완료!");
            }
        }
    }

    private int GetAvailableSeatCount()
    {
        int count = 0;
        for (int i = 0; i < seatStates.Length; i++)
        {
            if (!seatStates[i]) count++;
        }
        return count;
    }

    private void SpawnRandomCustomer()
    {
    
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < seatStates.Length; i++)
        {
            if (!seatStates[i]) availableIndices.Add(i);
        }

        if (availableIndices.Count == 0) return; 

        
        int randIdx = Random.Range(0, availableIndices.Count); 
        int finalSeatIndex = availableIndices[randIdx];        


        seatStates[finalSeatIndex] = true;
        Transform assignedSeat = seatPoints[finalSeatIndex];

      
        int randomDataIndex = Random.Range(0, allCustomers.Count);
        CustomerData selectedData = allCustomers[randomDataIndex];

       
        GameObject npcObj = Instantiate(selectedData.npcPrefab, spawnPoint.position, Quaternion.identity);

        CustomerController controller = npcObj.GetComponent<CustomerController>();
        if (controller == null) controller = npcObj.AddComponent<CustomerController>();

       
        controller.Initialize(selectedData, counterPoint, exitPoint, assignedSeat, this);
    }

   
    public void ReturnSeat(Transform seatTrans)
    {
       
        int index = seatPoints.IndexOf(seatTrans);

        if (index != -1)
        {
            seatStates[index] = false;
            Debug.Log($"좌석 {index}번이 비워졌습니다. (다음 손님 가능)");
        }
    }
}
