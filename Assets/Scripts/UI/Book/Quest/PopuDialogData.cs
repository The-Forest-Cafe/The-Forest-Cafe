using UnityEngine;

[System.Serializable]
public class PopuQuestProgressDialog
{
    public string questID;  //예: "POPU_STAR_5", "POPU_GHOST_5", "POPU_HOT_5"

    [TextArea(2, 4)] public string[] linesAt0;          //0잔
    [TextArea(2, 4)] public string[] linesAt1;          //1잔
    [TextArea(2, 4)] public string[] linesAt2;          //2잔
    [TextArea(2, 4)] public string[] linesAt3;          //3잔
    [TextArea(2, 4)] public string[] linesAt4;          //4잔
    [TextArea(2, 4)] public string[] linesAt5;          //5잔 달성 순간 (최초 클리어용)
    [TextArea(2, 4)] public string[] linesAfterClear;   //그 이후 공통 대사

    public string[] GetLines(int servedCount, bool isCleared)
    {
        //클리어된 상태면 항상 동일 대사
        if (isCleared)
        {
            if (linesAfterClear != null && linesAfterClear.Length > 0)
                return linesAfterClear;

            //없으면 5잔 대사라도 사용
            if (linesAt5 != null && linesAt5.Length > 0)
                return linesAt5;
        }

        //아직 클리어 전이면 잔 수에 따라
        switch (servedCount)
        {
            case 0: return linesAt0;
            case 1: return linesAt1;
            case 2: return linesAt2;
            case 3: return linesAt3;
            case 4: return linesAt4;
            case 5: return linesAt5;
            default:
                //이상한 값이 오면 그냥 afterClear나 5잔 대사 사용
                if (linesAfterClear != null && linesAfterClear.Length > 0)
                    return linesAfterClear;
                return linesAt5;
        }
    }
}

[CreateAssetMenu(menuName = "Dialog/Popu Dialog")]
public class PopuDialogData : ScriptableObject
{
    public PopuQuestProgressDialog[] quests;

    public string[] GetLines(string questID, int servedCount, bool isCleared)
    {
        foreach (var q in quests)
        {
            if (q.questID == questID)
            {
                return q.GetLines(servedCount, isCleared);
            }
        }

        Debug.LogWarning($"PopuMultiQuestDialogData: questID {questID} 에 해당하는 대사가 없음");
        return null;
    }
}
