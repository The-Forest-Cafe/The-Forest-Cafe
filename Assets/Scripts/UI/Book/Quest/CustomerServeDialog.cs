using System.Linq;
using UnityEngine;

[System.Serializable]
public class CustomerPanelByName
{
    [Header("손님 이름 (myData.npcName 과 일치)")]
    public string customerName;      
    [Header("이 손님 전용 대화 패널 이미지")]
    public Sprite panelSprite;
}

public class CustomerServeDialog : MonoBehaviour
{
    public static CustomerServeDialog Instance { get; private set; }

    [Header("공통 랜덤 대사 (모든 일반 손님 공용)")]
    [TextArea(2, 3)]
    public string[] randomLines;         //공통 대사

    [Header("손님별 패널 매핑")]
    public CustomerPanelByName[] customers;    //4명 손님 등록

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
    /// 일반 손님에게 서빙 성공했을 때 호출.
    /// customerName : npc.myData.npcName
    /// </summary>
    public void ShowRandomServeDialog(string customerName)
    {
        if (DialogManager.Instance == null)
        {
            Debug.LogWarning("CustomerServeDialog : DialogManager 없음");
            return;
        }

        if (randomLines == null || randomLines.Length == 0)
        {
            Debug.LogWarning("CustomerServeDialog : randomLines 비어있음");
            return;
        }

        //1) 손님 이름으로 패널 찾기
        Sprite panel = defaultPanelSprite;
        if (!string.IsNullOrEmpty(customerName))
        {
            foreach (var c in customers)
            {
                if (c != null && c.customerName == customerName)
                {
                    if (c.panelSprite != null)
                        panel = c.panelSprite;
                    break;
                }
            }
        }

        //2) 공통 랜덤 대사 하나 뽑기
        int idx = Random.Range(0, randomLines.Length);
        string line = randomLines[idx];

        //3) 패널 + 랜덤 대사로 대화창 띄우기
        DialogManager.Instance.Show(new string[] { line }, panel);
    }
}
