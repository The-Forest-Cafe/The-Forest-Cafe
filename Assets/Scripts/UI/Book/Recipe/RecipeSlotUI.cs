using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    [Header("Data")]
    public string recipeId;              //이 슬롯이 나타내는 레시피 ID

    [Header("UI")]
    public GameObject lockOverlay;       //잠금 이미지+버튼 들어있는 오브젝트
    public Button unlockButton;          //잠금 상태일 때 클릭하는 버튼

    void Start()
    {
        Refresh();

        if (unlockButton != null)
        {
            unlockButton.onClick.AddListener(OnClickUnlock);
        }

        // 레시피가 다른 곳에서 해금되었을 때 반영
        if (RecipeManager.Instance != null)
        {
            RecipeManager.Instance.OnRecipeUnlocked += OnRecipeUnlocked;
        }
    }

    void OnDestroy()
    {
        if (RecipeManager.Instance != null)
        {
            RecipeManager.Instance.OnRecipeUnlocked -= OnRecipeUnlocked;
        }
    }

    void OnRecipeUnlocked(RecipeData data)
    {
        if (data.recipeId == recipeId)
        {
            Refresh();
        }
    }

    void Refresh()
    {
        bool isUnlocked = RecipeManager.Instance.IsUnlocked(recipeId);

        //잠금 오버레이 On/Off
        if (lockOverlay != null)
            lockOverlay.SetActive(!isUnlocked);
    }

    void OnClickUnlock()
    {
        if (RecipeManager.Instance.TryUnlockRecipe(recipeId))
        {
            //성공 시 UI 갱신
            Refresh();
        }
        else
        {
            //돈이 부족하거나 이미 해금된 상태 등
            //TODO: "돈이 부족합니다" 같은 팝업 띄우기
            Debug.Log("레시피 해금 실패 혹은 돈 부족");
        }
    }
}
