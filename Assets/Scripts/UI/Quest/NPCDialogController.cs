using UnityEngine;

public class NPCDialogController : MonoBehaviour
{
    [Header("Dialog Data")]
    public DialogData dialogData;    //이 NPC의 대사 ScriptableObject

    [Header("Talk Range")]
    public float talkRange = 3f;     // NPC와 플레이어 사이 거리 조건

    Transform player;

    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    bool IsPlayerInRange()
    {
        if (player == null) return false;

        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= talkRange;
    }

    // === 퀘스트 시스템에서 이 함수를 호출 ===
    public void OnQuestCompleted(string questID)
    {
        if (dialogData == null)
        {
            Debug.LogWarning($"{name} : dialogData 가 비어있음");
            return;
        }

        //플레이어가 가까이 있을 때만 대사 재생
        if (!IsPlayerInRange())
        {
            Debug.Log($"{name} : 퀘스트 {questID} 클리어지만 플레이어가 범위 밖");
            return;
        }

        string[] lines = dialogData.GetLines(questID);
        if (lines == null || lines.Length == 0)
            return;

        DialogManager.Instance.Show(lines);
    }

}
