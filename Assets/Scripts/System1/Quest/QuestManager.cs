using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("현재 활성화된 퀘스트 목록")]
    public List<QuestData> activeQuests;

    [Header("대사 컨트롤러")]
    public PopuDialogController popuController;   
    public NPCDialogController eebulController;   
    public NPCDialogController yonyuController;  
    public NPCDialogController baneulController;  

    public event Action<QuestData> OnQuestUpdated;
    public event Action<QuestData> OnQuestCompleted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (var quest in activeQuests)
        {
            if (quest != null) quest.ResetProgress();
        }
    }

    public void UpdateQuestProgress(CustomerController customerInstance, Recipe servedDrink)
    {
        CustomerData customerData = customerInstance.myData;
        Debug.Log($"손님: {customerData.npcName}, 음료: {servedDrink.drinkName}");

        //일반 손님
        if (customerData.type == CustomerType.General)
        {
            var dialog = customerInstance.GetComponent<CustomerServeDialog>();
            if (dialog != null)
            {
              //  dialog.OnServeSuccess();
            }
            return;
        }


        //NPC
        foreach (var quest in activeQuests)
        {
            if (quest.isCompleted) continue;

            if (quest.targetCustomer != null && quest.targetCustomer != customerData) continue;

            bool isMatch = CheckCondition(quest, servedDrink);

            if (isMatch)
            {
                // 카운트 증가
                quest.currentCount++;
                Debug.Log($"퀘스트 진행! ID:{quest.questID} ({quest.currentCount}/{quest.goalCount})");

                OnQuestUpdated?.Invoke(quest);

                bool isJustFinished = false;
                if (quest.currentCount >= quest.goalCount && !quest.isCompleted)
                {
                    quest.isCompleted = true;
                    isJustFinished = true;
                    OnQuestCompleted?.Invoke(quest);
                }

                //NPC별 대사
                HandleDialogInteraction(customerData.npcName, quest, isJustFinished);
            }
        }
    }

    private bool CheckCondition(QuestData quest, Recipe servedDrink)
    {
        if (quest.targetRecipe != null)
            return quest.targetRecipe == servedDrink;
        else if (quest.targetTag != DrinkTag.None)
            return servedDrink.drinkTags.Contains(quest.targetTag);
        else
            return true;
    }

    private void HandleDialogInteraction(string npcName, QuestData quest, bool isJustFinished)
    {
        //포푸
        if (npcName == "포푸" && popuController != null)
        {
           
            popuController.OnPopuQuestStateChanged(quest.questID, quest.currentCount, quest.isCompleted);
        }
        // 2. 이불
        else if (npcName == "이불" && eebulController != null)
        {
            if (isJustFinished) eebulController.OnQuestCompleted(quest.questID);
        }
        // 3. 연유
        else if (npcName == "연유" && yonyuController != null)
        {
            if (isJustFinished) yonyuController.OnQuestCompleted(quest.questID);
        }
        // 4. 바늘
        else if (npcName == "바늘" && baneulController != null)
        {
            if (isJustFinished) baneulController.OnQuestCompleted(quest.questID);
        }
    }
}
