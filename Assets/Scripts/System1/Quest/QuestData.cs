using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest", menuName = "Cafe/Quest")]
public class QuestData : ScriptableObject
{
    [Header("퀘스트 식별자")]
    public string questID;

    [Header("퀘스트 기본 정보")]
    public string questTitle;
    [TextArea] public string description;

    [Header("목표 설정 (조건)")]
    public CustomerData targetCustomer;
    public Recipe targetRecipe;
    public DrinkTag targetTag;

    [Header("카운트 설정")]
    public int goalCount;
    public int currentCount;
    public bool isCompleted;

    public void ResetProgress()
    {
        currentCount = 0;
        isCompleted = false;
    }
}
