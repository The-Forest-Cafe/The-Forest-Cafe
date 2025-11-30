using UnityEngine;
using System.Collections.Generic;

public enum DrinkTag
{
    None, 
    Hot,  
    Cold,     
    Smoothie,   
    BubbleTea,  
    Latte       
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Cafe/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeID;

    public string drinkName;            // 완성 음료 이름
    public Sprite drinkIcon;            // 완성 시 음료 이미지
    public GameObject drinkPrefab;      //3D 모델
    public int price;                   // 음료 가격

    [Header("음료 속성")]
    public List<DrinkTag> drinkTags;

    public Ingredient baseIngredient;   // 필수 베이스
    public Ingredient baseWay;
    public List<Ingredient> ingredients; // 추가 재료 리스트
}
