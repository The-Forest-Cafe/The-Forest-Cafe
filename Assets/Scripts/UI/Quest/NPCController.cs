using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Quest / Dialog")]
    public string npcQuestID;       // 이 NPC와 연결된 퀘스트 ID (예: "Serve_NPC01")
    public DialogData dialogData;   // 이 NPC용 대사 데이터 (ScriptableObject)
    public string npcName;          // 대사창에 표시할 NPC 이름

    [Header("Talk Range")]
    public float talkRange = 3f;    // 플레이어와의 거리

    Transform player;
    bool hasPlayedDialog = false;   // 한 번 대사 재생 후 다시 안 나오게 하려면 사용

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player 태그 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // 괄호 빠져 있던 부분 수정 + 이미 한 번 말했으면 패스
        if (!hasPlayedDialog &&
            dist < talkRange &&
            QuestManager.Instance != null &&
            QuestManager.Instance.IsComplete(npcQuestID))
        {
            TriggerDialog();
        }
    }

    void TriggerDialog()
    {
        var lines = GetDialogByQuest(npcQuestID);
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning($"NPC {name} : questID {npcQuestID}에 해당하는 대사가 없음");
            return;
        }

        // DialogManager의 StartDialog 사용 (Show 아님!)
        DialogManager.Instance.StartDialog(lines, npcName);
        hasPlayedDialog = true;
    }

    string[] GetDialogByQuest(string questID)
    {
        if (dialogData == null || dialogData.dialogs == null)
            return null;

        foreach (var d in dialogData.dialogs)
        {
            if (d.questID == questID)
                return d.lines;
        }

        return null;
    }
}
