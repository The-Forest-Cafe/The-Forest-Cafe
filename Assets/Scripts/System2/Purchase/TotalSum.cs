using UnityEngine;
using UnityEngine.UI;

public class TotalSum : MonoBehaviour
{
    public static TotalSum Instance;
    public int totalSum;

    [SerializeField]
    private Text totalSumText;

    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    private void Start()
    {
        InitTotalSum();
        PurchaseManager.Instance.onPurchaseListInited += InitTotalSum;
    }

    private void InitTotalSum()
    {
        totalSum = 0;
        SetSumText();
    }

    public void SetSumText()
    {
        totalSumText.text = $"{totalSum.ToString()}";
    }
}
