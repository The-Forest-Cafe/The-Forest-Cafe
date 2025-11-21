using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class DrinkMakingManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject baseSelectionPanel;
    public GameObject ingredientSelectionPanel;
    public Image drinkResultImage;

    [Header("Data")]
    public List<Recipe> allRecipes;
    public Recipe failedDrink;
    public GameObject playerHand;

    private Ingredient currentBase;
    private List<Ingredient> currentIngredients = new List<Ingredient>();
    private List<Image> toggledButtons = new List<Image>();

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
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

                if (toggledButtons.Contains(btnImage))
                    toggledButtons.Remove(btnImage);
            }
        }
        else
        {
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
        baseSelectionPanel.SetActive(false);
        ingredientSelectionPanel.SetActive(false);

        if (drinkResultImage != null && result.drinkIcon != null)
        {
            drinkResultImage.sprite = result.drinkIcon;
            drinkResultImage.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2.0f);
        /*
        if (playerHand != null && result.drinkPrefab != null)
        {
            foreach (Transform child in playerHand.transform)
                Destroy(child.gameObject);

            GameObject obj = Instantiate(result.drinkPrefab, playerHand.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }*/

        ClosePanel();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
