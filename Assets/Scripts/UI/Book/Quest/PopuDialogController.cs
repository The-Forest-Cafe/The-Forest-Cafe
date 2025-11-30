using UnityEngine;

public class PopuDialogController : MonoBehaviour
{
    [Header("Dialog Data")]
    public PopuDialogData dialogData;

    [Header("Talk Range")]
    public float talkRange = 3f;

    [Header("포푸 기본 대화창 이미지 (퀘스트별 스킨 없을 때)")]
    public Sprite defaultDialogSprite;

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

    /// <summary>
    /// 포푸 관련 퀘스트의 상태가 변할 때 호출.
    /// questID     : 어떤 퀘스트인지 (POPU_STAR_5, POPU_GHOST_5, POPU_HOT_5 등)
    /// servedCount : 지금까지 판 잔 수 (0~5 이상)
    /// isCleared   : 해당 퀘스트 클리어 여부
    /// </summary>
    public void OnPopuQuestStateChanged(string questID, int servedCount, bool isCleared)
    {
        if (dialogData == null)
        {
            Debug.LogWarning("PopuDialogController : dialogData 가 비어있음");
            return;
        }

        //플레이어가 가까이 있을 때만 대사 재생
        if (!IsPlayerInRange())
        {
            Debug.Log("Popu : 플레이어가 범위 밖이라 대사 재생 안함");
            return;
        }

        var lines = dialogData.GetLines(questID, servedCount, isCleared);
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("Popu : 해당 상태에 대한 대사가 없음");
            return;
        }

        DialogManager.Instance.Show(lines);

        // 2) 이 퀘스트/진행도에 맞는 대화창 이미지 가져오기
        Sprite sprite = dialogData.GetSprite(questID, servedCount, isCleared);

        // 3) 없으면 포푸 기본 스킨 사용
        if (sprite == null)
            sprite = defaultDialogSprite;

        // 4) 스킨까지 함께 넘겨서 대화창 띄우기
        DialogManager.Instance.Show(lines, sprite);
    }
}
