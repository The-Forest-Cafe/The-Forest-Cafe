using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("현재 활성화된 퀘스트 목록")]
    public List<QuestData> activeQuests;

    public event Action<QuestData> OnQuestUpdated;
    public event Action<QuestData> OnQuestCompleted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 게임 시작 시 초기화
        foreach (var quest in activeQuests)
        {
            if (quest != null) quest.ResetProgress();
        }
    }

    //서빙
    public void UpdateQuestProgress(CustomerData customer, Recipe servedDrink)
    {
        Debug.Log($"[퀘스트 검사] 손님: {customer.npcName}, 음료: {servedDrink.drinkName}");

        foreach (var quest in activeQuests)
        {
            if (quest.isCompleted) continue;

            if (quest.targetCustomer != null && quest.targetCustomer != customer) continue;

            bool isMatch = false;

            // 조건 확인
            if (quest.targetRecipe != null)
            {
                if (quest.targetRecipe == servedDrink) isMatch = true;
            }
            else if (quest.targetTag != DrinkTag.None)
            {
                if (servedDrink.drinkTags.Contains(quest.targetTag)) isMatch = true;
            }
            else
            {
                isMatch = true; // NPC만 맞으면 통과
            }

            // 조건 달성
            if (isMatch)
            {
                quest.currentCount++;
                Debug.Log($"퀘스트 진행 [{quest.questTitle}] {quest.currentCount}/{quest.goalCount}");

                OnQuestUpdated?.Invoke(quest);

                if (quest.currentCount >= quest.goalCount)
                {
                    CompleteQuest(quest);
                }
            }
        }
    }

    private void CompleteQuest(QuestData quest)
    {
        quest.isCompleted = true;
        Debug.Log($"퀘스트 완료 [{quest.questTitle}]");
        OnQuestCompleted?.Invoke(quest);
    }
}
