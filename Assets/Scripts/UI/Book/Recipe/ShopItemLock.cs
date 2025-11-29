using UnityEngine;
using UnityEngine.UI;

public class ShopItemLock : MonoBehaviour
{
    [Header("이 슬롯이 담당하는 재료 ID")]
    public string ingredientId;          //예: "ghost_syrup"

    [Header("UI 요소")]
    public GameObject lockOverlay;       //잠금 이미지/패널
    public SelectButton selectButton;    //팀원이 만든 선택 버튼

    void Start()
    {
        Refresh();

        if (IngredientUnlockManager.Instance != null)
        {
            IngredientUnlockManager.Instance.OnIngredientUnlocked += OnIngredientUnlocked;
        }
    }

    void OnDestroy()
    {
        if (IngredientUnlockManager.Instance != null)
        {
            IngredientUnlockManager.Instance.OnIngredientUnlocked -= OnIngredientUnlocked;
        }
    }

    void OnIngredientUnlocked(string unlockedId)
    {
        if (unlockedId == ingredientId)
        {
            Refresh();
        }
    }

    void Refresh()
    {
        bool unlocked = IngredientUnlockManager.Instance != null
                     && IngredientUnlockManager.Instance.IsUnlocked(ingredientId);

        //잠금 오버레이 On/Off
        if (lockOverlay != null)
            lockOverlay.SetActive(!unlocked);

        //선택 버튼 비활성/활성
        if (selectButton != null)
        {
            var btn = selectButton.GetComponent<Button>();
            if (btn != null)
                btn.interactable = unlocked;
        }
    }
}
