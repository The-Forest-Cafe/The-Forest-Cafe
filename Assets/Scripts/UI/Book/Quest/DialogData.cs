using UnityEngine;

[System.Serializable]
public class QuestDialog
{
    public string questID;      //예: "EEBUL_GHOST_5"
    [TextArea(2, 5)]
    public string[] lines;      //한 줄씩 대사
    public Sprite dialogSprite;
}

[CreateAssetMenu(menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    public string npcID;        //예: "EEBUL"
    public QuestDialog[] dialogs;

    // 퀘스트 ID로 대사 배열 찾는 함수
    public string[] GetLines(string questID)
    {
        foreach (var d in dialogs)
        {
            if (d.questID == questID)
                return d.lines;
        }

        Debug.LogWarning($"DialogData: questID {questID} 에 해당하는 대사가 없음");
        return null;
    }

    public Sprite GetSprite(string questID)
    {
        foreach (var d in dialogs)
        {
            if (d.questID == questID)
                return d.dialogSprite;
        }
        return null;
    }
}
