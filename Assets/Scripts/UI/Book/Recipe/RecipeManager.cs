using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }

    [Header("전체 레시피 목록")]
    public List<RecipeData> recipes;

    [Header("최종 레시피 조각")]
    public int finalRecipePieces = 0;   //0~4

    [Header("최종 레시피 완성 시 보상 재료 ID들")]
    public string[] finalRewardIngredientIds;

    // 내부 상태
    Dictionary<string, bool> unlockedMap = new Dictionary<string, bool>();
    bool finalRewardGiven = false;   //보상 중복 지급 방지용

    public System.Action<RecipeData> OnRecipeUnlocked;       //레시피 해금 이벤트
    public System.Action<int> OnFinalPiecesChanged;          //최종레시피 조각 변경 이벤트

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        InitUnlockState();
    }

    void InitUnlockState()
    {
        unlockedMap.Clear();
        foreach (var r in recipes)
        {
            bool unlocked = r.unlockedByDefault;
            unlockedMap[r.recipeId] = unlocked;

            //처음부터 열려있는 레시피 재료는 바로 해금
            if (unlocked)
            {
                UnlockIngredientsOfRecipe(r);
            }
        }
    }

    public bool IsUnlocked(string recipeId)
    {
        return unlockedMap.ContainsKey(recipeId) && unlockedMap[recipeId];
    }

    public RecipeData GetRecipe(string recipeId)
    {
        return recipes.Find(r => r.recipeId == recipeId);
    }

    /// <summary>
    /// 돈 차감 후 레시피 해금 시도
    /// </summary>
    public bool TryUnlockRecipe(string recipeId)
    {
        if (!unlockedMap.ContainsKey(recipeId))
            return false;

        if (unlockedMap[recipeId])
            return false; //이미 해금

        RecipeData data = GetRecipe(recipeId);
        if (data == null)
            return false;

        //돈 검사 (MoneyManager는 이미 있을 거라고 가정)
        bool paid = MoneyManager.Instance.SpendMoney(data.unlockCost);
        if (!paid)
        {
            //돈 부족 -> 해금 실패
            // 여기서 "돈이 부족합니다" 팝업 띄우면 됨
            Debug.Log("레시피 해금 실패: 돈 부족");
            return false;
        }

        unlockedMap[recipeId] = true;
        UnlockIngredientsOfRecipe(data);

        OnRecipeUnlocked?.Invoke(data);

        // TODO: 세이브 저장하고 싶으면 여기서 처리
        return true;
    }

    void UnlockIngredientsOfRecipe(RecipeData recipe)
    {
        foreach (var ingId in recipe.ingredientIds)
        {
            IngredientUnlockManager.Instance.UnlockById(ingId);
        }
    }

    /// <summary>
    /// NPC들이 주문할 수 있는 레시피 목록
    /// </summary>
    public List<RecipeData> GetUnlockedRecipes()
    {
        List<RecipeData> list = new List<RecipeData>();
        foreach (var r in recipes)
        {
            if (IsUnlocked(r.recipeId))
                list.Add(r);
        }
        return list;
    }

    /// <summary>
    /// NPC 퀘스트 올클 시 불러줄 함수 (조각 1개 획득)
    /// </summary>
    public void AddFinalRecipePiece()
    {
        int old = finalRecipePieces;
        finalRecipePieces = Mathf.Clamp(finalRecipePieces + 1, 0, 4);
        if (finalRecipePieces != old)
        {
            OnFinalPiecesChanged?.Invoke(finalRecipePieces);
        }

        //최종 레시피 완성(4조각) 시 보상 재료 해금
        if (finalRecipePieces == 4 && !finalRewardGiven)
        {
            finalRewardGiven = true;

            SFXManager.Instance.Play("final_solve");

            if (IngredientUnlockManager.Instance != null && finalRewardIngredientIds != null)
            {
                foreach (var id in finalRewardIngredientIds)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        IngredientUnlockManager.Instance.UnlockById(id);
                    }
                }
            }

            Debug.Log("최종 레시피 완성! 보상 재료 해금 완료");
        }

    }
}
