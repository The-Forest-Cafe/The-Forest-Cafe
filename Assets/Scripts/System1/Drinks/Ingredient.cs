using UnityEngine;

public enum IngredientType { Base, Crop, Bean, Etc, Way }

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Cafe/Ingredient")]

public class Ingredient : ScriptableObject
{

    public string ingredientName;    // 재료 이름
    public Sprite icon;              // UI에 표시될 아이콘
    public IngredientType type;      // 재료 타입
    public PurchaseObjectSO purchaseData; // 인벤토리 아이템 데이터
}
