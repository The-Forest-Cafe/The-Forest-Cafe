using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager Instance;
    public PlayerInventory player;

    public delegate void OnPurchaseListInited();
    public event OnPurchaseListInited onPurchaseListInited;

    [SerializeField]
    private UnityEngine.UI.Text noticeText;    // 돈 부족/충족 시 노출되는 텍스트
    private List<PurchaseObjectSO> purchaseList;

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

        noticeText.color = new Color(255, 255, 255, 0);
        InitList();
        onPurchaseListInited += InitList;
    }

    private void InitList()
    {
        purchaseList = new();
    }

    public void AddList(PurchaseObjectSO material)
    {
        purchaseList.Add(material);
    }

    public void RemoveList(PurchaseObjectSO material)
    {
        purchaseList.Remove(material);
    }

    public void OnPurchaseButtonClicked()
    {
        bool res = MoneyManager.Instance.SpendMoney(TotalSum.Instance.totalSum);
        SetNoticeText(res);

        // 돈 확인
        if (!res) return;

        foreach (PurchaseObjectSO material in purchaseList)
        {
            int price = material.price;

            // 재료 추가
            player.AddMaterial(material, material.quantity);
        }

        onPurchaseListInited?.Invoke();

        // TestCode
        player.TestToShowList();
    }

    public void OnPurchasePanelClosed()
    {
        onPurchaseListInited?.Invoke();
    }

    private void SetNoticeText(bool res)
    {
        if (res)
        {
            noticeText.color = new Color32(44, 135, 40, 255);
            noticeText.text = "재료 구매 완료!";
        }
        else
        {
            noticeText.color = Color.red;
            noticeText.text = "소지한 돈이 충분하지 않습니다!";
        }

        StartCoroutine(TextFadeIn(.2f));
    }

    public IEnumerator TextFadeIn(float duration)
    {
        float t = 0f;
        Color c = noticeText.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duration);
            noticeText.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        StartCoroutine(TextFadeOut(.2f));
    }

    public IEnumerator TextFadeOut(float duration)
    {
        float t = 0f;
        Color c = noticeText.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / duration);
            noticeText.color = c;
            yield return null;
        }
    }
}
