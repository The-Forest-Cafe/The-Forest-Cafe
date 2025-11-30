using UnityEngine;

public class NPCDialogController : MonoBehaviour
{
    public DialogData dialogData;


    // === 퀘스트 시스템에서 이 함수를 호출 ===
    public void OnQuestCompleted(string questID)
    {
        if (dialogData == null)
        {
            Debug.LogWarning($"{name} : dialogData 가 비어있음");
            return;
        }

        string[] lines = dialogData.GetLines(questID);
        if (lines == null || lines.Length == 0)
            return;

        Sprite sprite = dialogData.GetSprite(questID);

        DialogManager.Instance.Show(lines, sprite);
    }

}
