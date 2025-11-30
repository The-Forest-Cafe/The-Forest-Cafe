using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class IngredientUIItem
{
    public string label;            // (구분용 메모) 예: 허브 버튼
    public Ingredient ingredient;   // 연결할 재료 데이터
    public Text buttonText;         // 개수를 표시할 버튼의 텍스트 컴포넌트
}

public class DrinkMakingManager : MonoBehaviour
{
    public static DrinkMakingManager Instance;

    [Header("UI Panels")]
    public GameObject baseSelectionPanel;
    public GameObject ingredientSelectionPanel;
    public Image drinkResultImage;

    [Header("Data")]
    public List<Recipe> allRecipes;
    public Recipe failedDrink;
    public GameObject playerHand;

    [Header("--- [Team Request] Inventory & Result ---")]
    public Recipe currentDrink;
    public PlayerInventory playerInventory;

    [Header("--- [UI Update] 재료 개수 표시 설정 ---")]
    // ★ 여기에 재료와 텍스트를 짝지어서 등록해주세요! ★
    public List<IngredientUIItem> ingredientUIs;

    private Ingredient currentBase;
    private List<Ingredient> currentIngredients = new List<Ingredient>();
    private List<Image> toggledButtons = new List<Image>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void OnEnable()
    {
        foreach (var img in toggledButtons)
        {
            if (img != null)
            {
                Color c = img.color;
                c.a = 1f;
                img.color = c;
            }
        }
        toggledButtons.Clear();

        currentBase = null;
        currentIngredients.Clear();

        if (baseSelectionPanel != null) baseSelectionPanel.SetActive(true);
        if (ingredientSelectionPanel != null) ingredientSelectionPanel.SetActive(false);

        if (drinkResultImage != null)
            drinkResultImage.gameObject.SetActive(false);

        // [추가됨] 창이 열릴 때 재료 개수 갱신
        UpdateIngredientCounts();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    // ★ 인벤토리 정보를 받아와서 UI 텍스트를 바꿔주는 함수 ★
    private void UpdateIngredientCounts()
    {
        if (playerInventory == null) return;

        foreach (var item in ingredientUIs)
        {
            // 데이터와 텍스트가 모두 잘 연결되어 있다면
            if (item.ingredient != null && item.buttonText != null)
            {
                // 1. 이름 가져오기
                string name = item.ingredient.ingredientName;

                // 2. 개수 가져오기 (구매 데이터가 없으면 무제한 취급하거나 0개)
                int count = 0;
                if (item.ingredient.purchaseData != null)
                {
                    count = playerInventory.GetMaterialAmount(item.ingredient.purchaseData);
                }

                // 3. 텍스트 변경: "밤하늘 허브\n(5개)"
                // 구매 데이터가 없는 베이스 재료 등은 개수 표시 안 함
                if (item.ingredient.purchaseData != null)
                {
                    item.buttonText.text = $"({count})";
                }
                else
                {
                    item.buttonText.text = name; // 개수 없이 이름만
                }
            }
        }
    }

    public void SelectBase(Ingredient baseSO)
    {
        currentBase = baseSO;
        baseSelectionPanel.SetActive(false);
        ingredientSelectionPanel.SetActive(true);
    }

    public void AddIngredient(Ingredient ingredientSO)
    {
        GameObject clickedBtn = EventSystem.current.currentSelectedGameObject;
        if (clickedBtn == null) return;

        Image btnImage = clickedBtn.GetComponent<Image>();

        if (currentIngredients.Contains(ingredientSO))
        {
            currentIngredients.Remove(ingredientSO);
            if (btnImage != null)
            {
                Color c = btnImage.color;
                c.a = 1f;
                btnImage.color = c;
                if (toggledButtons.Contains(btnImage)) toggledButtons.Remove(btnImage);
            }
        }
        else
        {
            if (playerInventory != null && ingredientSO.purchaseData != null)
            {
                int currentStock = playerInventory.GetMaterialAmount(ingredientSO.purchaseData);
                int amountInUse = currentIngredients.Count(x => x == ingredientSO);

                if (amountInUse >= currentStock)
                {
                    Debug.Log($"[재료 부족] {ingredientSO.ingredientName} 재고 부족!");
                    return;
                }
            }

            currentIngredients.Add(ingredientSO);
            if (btnImage != null)
            {
                Color c = btnImage.color;
                c.a = 0.5f;
                btnImage.color = c;
                toggledButtons.Add(btnImage);
            }
        }
    }

    public void OnCompleteButton()
    {
        Recipe result = CheckRecipe();
        currentDrink = result;
        Debug.Log($"[Manager] 음료 완성 및 저장됨: {currentDrink.drinkName}");

        StartCoroutine(ShowDrinkResult(result));
    }

    private Recipe CheckRecipe()
    {
        foreach (Recipe recipe in allRecipes)
        {
            if (recipe.baseIngredient != currentBase) continue;
            if (recipe.ingredients.Count != currentIngredients.Count) continue;
            bool allMatch = recipe.ingredients.All(item => currentIngredients.Contains(item));

            if (allMatch) return recipe;
        }
        return failedDrink;
    }

    private IEnumerator ShowDrinkResult(Recipe result)
    {
        if (playerInventory != null)
        {
            foreach (var ing in currentIngredients)
            {
                if (ing != null && ing.purchaseData != null)
                {
                    playerInventory.AddMaterial(ing.purchaseData, -1);
                }
            }
        }

        // [추가됨] 재료를 썼으니 개수를 다시 갱신해서 보여줌
        UpdateIngredientCounts();

        baseSelectionPanel.SetActive(false);
        ingredientSelectionPanel.SetActive(false);

        if (drinkResultImage != null && result.drinkIcon != null)
        {
            drinkResultImage.sprite = result.drinkIcon;
            drinkResultImage.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2.0f);

        if (playerHand != null && result.drinkPrefab != null)
        {
            foreach (Transform child in playerHand.transform)
                Destroy(child.gameObject);

            GameObject obj = Instantiate(result.drinkPrefab, playerHand.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }

        ClosePanel();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}