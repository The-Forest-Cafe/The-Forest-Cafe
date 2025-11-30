using System.Linq;
using UnityEngine;

public class CustomerServeDialog : MonoBehaviour
{
    public static CustomerServeDialog Instance { get; private set; }

    [System.Serializable]
    public class CustomerPanel
    {
        [Header("이 손님 오브젝트")]
        public GameObject customer;      // 해당 손님 NPC

        [Header("이 손님 전용 대화 패널 이미지")]
        public Sprite panelSprite;       // 이 손님의 패널 1개
    }

    [Header("공통 랜덤 대사 (모든 일반 손님 공용)")]
    [TextArea(2, 3)]
    public string[] randomLines;         // 7개 정도 공통 대사

    [Header("손님별 패널 매핑")]
    public CustomerPanel[] customers;    // 4명 손님 등록

    [Header("매핑 안 됐을 때 사용할 기본 패널 (선택)")]
    public Sprite defaultPanelSprite;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// 이 손님에게 서빙에 성공했을 때 호출
    /// </summary>
    public void ShowRandomServeDialog(GameObject customerObj)
    {
        if (DialogManager.Instance == null)
        {
            Debug.LogWarning("GenericCustomerDialogManager : DialogManager.Instance 없음");
            return;
        }

        if (customerObj == null)
        {
            Debug.LogWarning("GenericCustomerDialogManager : customerObj 가 null임");
            return;
        }

        // 1) 손님 오브젝트에 해당하는 패널 찾기
        var data = customers.FirstOrDefault(c => c.customer == customerObj);

        Sprite panelSprite = defaultPanelSprite;
        if (data != null && data.panelSprite != null)
            panelSprite = data.panelSprite;

        // 2) 공통 랜덤 대사에서 한 줄 뽑기
        int idx = Random.Range(0, randomLines.Length);
        string line = randomLines[idx];

        // 3) 그 패널 + 랜덤 한 줄로 대화창 띄우기
        DialogManager.Instance.Show(new string[] { line }, panelSprite);
    }
}
