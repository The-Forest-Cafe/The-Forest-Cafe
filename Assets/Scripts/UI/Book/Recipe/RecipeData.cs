using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/Recipe Data")]
public class RecipeData : ScriptableObject
{
    [Header("ID / 표시 이름")]
    public string recipeId;          //고유 ID (예: "starlight_tea_1")
    public string displayName;      //UI용 이름

    [Header("해금 관련")]
    public int unlockCost;          //해금 비용
    public bool unlockedByDefault;  //처음부터 열려있는 레시피인지

    [Header("재료")]
    public string[] ingredientIds;  //이 레시피에 필요한 재료 ID들
}
