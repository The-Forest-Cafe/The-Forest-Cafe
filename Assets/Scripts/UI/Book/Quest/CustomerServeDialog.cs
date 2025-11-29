using UnityEngine;

public class CustomerServeDialog : MonoBehaviour
{
    public static CustomerServeDialog Instance { get; private set; }

    [Header("Random Dialog")]
    [TextArea(2, 3)]
    public string[] randomLines;

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
    public void ShowRandomServeDialog()
    {
        if (DialogManager.Instance == null)
        {
            Debug.LogWarning("CustomerServeDialog : DialogManager.Instance 없음");
            return;
        }

        if (randomLines == null || randomLines.Length == 0)
        {
            Debug.LogWarning("CustomerServeDialog : randomLines 비어있음");
            return;
        }

        int idx = Random.Range(0, randomLines.Length);
        string line = randomLines[idx];

        DialogManager.Instance.Show(new string[] { line });
    }
}
