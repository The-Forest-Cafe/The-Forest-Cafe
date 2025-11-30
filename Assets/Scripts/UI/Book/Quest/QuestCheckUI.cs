using UnityEngine;

public class QuestCheckUI : MonoBehaviour
{
    [Header("이 체크박스가 연결된 퀘스트")]
    [SerializeField] QuestData targetQuest;

    [Header("체크 On 이미지")]
    [SerializeField] GameObject checkOnImage;

    void Awake()
    {
        // 처음 시작할 때는 퀘스트 완료 여부에 맞게 세팅
        if (checkOnImage != null)
            checkOnImage.SetActive(targetQuest != null && targetQuest.isCompleted);
    }

    void OnEnable()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnQuestCompleted += HandleQuestCompleted;
    }

    void OnDisable()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnQuestCompleted -= HandleQuestCompleted;
    }

    void HandleQuestCompleted(QuestData quest)
    {
        // 내가 담당하는 퀘스트가 완료됐을 때만 체크 On
        if (quest == targetQuest && checkOnImage != null)
        {
            checkOnImage.SetActive(true);
        }
    }
}
