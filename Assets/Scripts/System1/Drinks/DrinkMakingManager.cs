using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class IngredientUIItem
{
    public string label;         
    public Ingredient ingredient; 
    public Text buttonText;      
}

public class DrinkMakingManager : MonoBehaviour
{
    public static DrinkMakingManager Instance;

    [Header("UI Panels")]
    public GameObject baseSelectionPanel;
    public GameObject ingredientSelectionPanel;
    public Image drinkResultImage;

    [Header("실패 화면")]
    public GameObject failedResultPanel;

    [Header("Data")]
    public List<Recipe> allRecipes;
    public Recipe failedDrink;
    public GameObject playerHand;

    [Header("인벤토리")]
    public Recipe currentDrink;
    public PlayerInventory playerInventory;

    [Header("재료 개수")]
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

        if (failedResultPanel != null)
            failedResultPanel.SetActive(false);

        UpdateIngredientCounts();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    private void UpdateIngredientCounts()
    {
        if (playerInventory == null) return;

        foreach (var item in ingredientUIs)
        {
            if (item.ingredient != null && item.buttonText != null)
            {

                string name = item.ingredient.ingredientName;

                int count = 0;
                if (item.ingredient.purchaseData != null)
                {
                    count = playerInventory.GetMaterialAmount(item.ingredient.purchaseData);
                }


                if (item.ingredient.purchaseData != null)
                {
                    item.buttonText.text = $"({count})";
                }
                else
                {
                    item.buttonText.text = name;
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
                    Debug.Log($"[재료 부족] {ingredientSO.ingredientName} 재고 부족");
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
        Debug.Log($"음료 완성 및 저장: {currentDrink.drinkName}");

        playerInventory.gameObject.GetComponent<PlayerInteraction>().hasDrink = true;
        playerInventory.gameObject.GetComponent<PlayerInteraction>().SetDrinkModel();

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

        UpdateIngredientCounts();

        baseSelectionPanel.SetActive(false);
        ingredientSelectionPanel.SetActive(false);

        bool isFailed = (result.drinkName == "이상한 음료");

        if (isFailed)
        {
            if (failedResultPanel != null)
            {
                ScoreManager.Instance.ApplyPenalty(PenaltyType.WrongDrink, 7);
                failedResultPanel.SetActive(true);
            }
        }
        else
        {
            if (drinkResultImage != null && result.drinkIcon != null)
            {
                drinkResultImage.sprite = result.drinkIcon;
                drinkResultImage.gameObject.SetActive(true);
            }
        }

        yield return new WaitForSeconds(2.0f);

        ClosePanel();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}