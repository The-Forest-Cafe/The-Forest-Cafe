using UnityEngine;

public class QuestTest : MonoBehaviour
{
    [Header("테스트용")]
    public CustomerData testCustomer;
    public Recipe testDrink;          

    public void SimulateServing()
    {
        if (QuestManager.Instance != null)
        {
            GameObject dummyGO = new GameObject("Test_Dummy_Customer");

            CustomerController dummyController = dummyGO.AddComponent<CustomerController>();

            dummyController.myData = testCustomer;

            Debug.Log($"테스트: 가상의 [{testCustomer.npcName}] 손님에게 서빙 시뮬레이션 실행");

            // 4. 이제 '컨트롤러'를 넘겨주면 매니저가 받아줍니다.
            QuestManager.Instance.UpdateQuestProgress(dummyController, testDrink);

            // 5. 볼일 다 봤으니 가짜 손님은 바로 삭제합니다.
            Destroy(dummyGO);
        }
        else
        {
            Debug.LogError("QuestManager가 씬에 없습니다!");
        }
    }

/*
    private void Start()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestUpdated += (quest) =>
            {
                Debug.Log($"[UI] 퀘스트 갱신: {quest.questTitle} ({quest.currentCount}/{quest.goalCount})");
            };

            QuestManager.Instance.OnQuestCompleted += (quest) =>
            {
                Debug.Log($"[UI] 퀘스트 클리어 체크박스: {quest.questTitle}");
            };
        }
    }
    */
}
