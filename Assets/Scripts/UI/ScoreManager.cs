using UnityEngine;
using System;

public enum PenaltyType
{
    WrongServing,    //잘못된 서빙
    CustomerLeave,   //손님 이탈
    WrongDrink       //잘못된 음료 제작
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("점수 설정")]
    [SerializeField] int startScore = 100;   //시작 점수
    [SerializeField] int minScore = 0;       //최소 점수 (0점 아래로 안내려가게)

    public int CurrentScore { get; private set; }

    [Header("패널티 카운트")]
    public int wrongServingCount { get; private set; }
    public int customerLeaveCount { get; private set; }
    public int wrongDrinkCount { get; private set; }

    // 점수 변할 때 UI에서 쓰고 싶으면 구독해서 사용 가능
    public Action<int> OnScoreChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지

        ResetScore();
    }

    /// <summary>
    /// 새 게임 시작할 때 호출 (점수/카운트 초기화)
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = startScore;

        wrongServingCount = 0;
        customerLeaveCount = 0;
        wrongDrinkCount = 0;

        OnScoreChanged?.Invoke(CurrentScore);
    }

    /// <summary>
    /// 패널티 적용 (다른 팀원들이 여기만 호출하면 됨)
    /// </summary>
    public void ApplyPenalty(PenaltyType type, int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[ScoreManager] 패널티 점수는 0보다 커야 합니다. 전달된 값: {amount}");
            return;
        }

        // 점수 감소
        CurrentScore -= amount;
        if (CurrentScore < minScore)
            CurrentScore = minScore;

        // 카운트 증가
        switch (type)
        {
            case PenaltyType.WrongServing:
                wrongServingCount++;
                break;
            case PenaltyType.CustomerLeave:
                customerLeaveCount++;
                break;
            case PenaltyType.WrongDrink:
                wrongDrinkCount++;
                break;
        }

        Debug.Log($"[ScoreManager] 패널티 적용: {type}, -{amount}점 / 현재 점수: {CurrentScore}");

        OnScoreChanged?.Invoke(CurrentScore);
    }
}
