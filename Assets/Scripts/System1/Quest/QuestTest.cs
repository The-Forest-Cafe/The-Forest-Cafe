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
            Debug.Log("테스트: 서빙 시뮬레이션");
            QuestManager.Instance.UpdateQuestProgress(testCustomer, testDrink);
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
