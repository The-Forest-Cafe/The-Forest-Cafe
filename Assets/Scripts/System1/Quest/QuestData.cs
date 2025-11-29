using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest", menuName = "Cafe/Quest")]
public class QuestData : ScriptableObject
{
    [Header("퀘스트 기본 정보")]
    public string questTitle;       // 퀘스트 제목
    [TextArea] public string description;

    [Header("목표 설정")]
    public CustomerData targetCustomer;      // 누가 주문

    // 특정 메뉴를 원할 때
    public Recipe targetRecipe;

    // 특정 속성을 원할 때
    public DrinkTag targetTag;

    [Header("카운트 설정")]
    public int goalCount;           // 목표 잔 수
    public int currentCount;        
    public bool isCompleted;        

    // 진행도 초기화
    public void ResetProgress()
    {
        currentCount = 0;
        isCompleted = false;
    }
}
