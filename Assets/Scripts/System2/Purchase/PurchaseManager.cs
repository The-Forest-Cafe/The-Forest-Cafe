using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static PurchaseManager;

public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager Instance;
    public PlayerInventory player;

    private List<PurchaseObjectSO> purchaseList;

    public delegate void OnPurchaseListInited();
    public event OnPurchaseListInited onPurchaseListInited;

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
        // 돈 확인
        if (!MoneyManager.Instance.SpendMoney(TotalSum.Instance.totalSum))
            return;

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
}
