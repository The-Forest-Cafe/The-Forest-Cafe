using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingResultUI : MonoBehaviour
{
    [SerializeField] GameObject panelRoot;   //엔딩 패널 오브젝트

    [Header("결과 텍스트")]
    [SerializeField] Text gradeText;            //점수 등급
    [SerializeField] Text finalScoreText;       //총 점수
    [SerializeField] Text wrongServingText;     //잘못된 서빙 횟수
    [SerializeField] Text customerLeaveText;    //손님 이탈 횟수
    [SerializeField] Text wrongDrinkText;       //잘못된 음료 제작 횟수

    [Header("버튼")]
    [SerializeField] Button restartButton;      //새로 시작하기
    [SerializeField] Button continueButton;     //이어하기

    [Header("씬 이름 설정")]
    [SerializeField] string startSceneName = "StartScene";   //완전 처음으로
    [SerializeField] string continueSceneName = "GameScene"; //이어하기로 갈 씬(예: 메인 카페 씬)

    void Awake()
    {
        // 처음엔 꺼져있게
        if (panelRoot != null)
            panelRoot.SetActive(false);

        if (restartButton)
            restartButton.onClick.AddListener(OnClickRestart);

        if (continueButton)
            continueButton.onClick.AddListener(OnClickContinue);
    }

    public void OpenEndingPanel()
    {
        var sm = ScoreManager.Instance;
        if (sm == null)
        {
            Debug.LogWarning("[EndingResultUI] ScoreManager가 없습니다.");
            return;
        }

        //등급 계산
        string grade = GetGrade(sm.CurrentScore);

        //텍스트 채우기
        if (gradeText)
            gradeText.text = $"{grade}";

        if (finalScoreText)
            finalScoreText.text = $"최종 점수 : {sm.CurrentScore}";

        if (wrongServingText)
            wrongServingText.text = $"잘못된 서빙 : {sm.wrongServingCount}";

        if (customerLeaveText)
            customerLeaveText.text = $"손님 이탈 : {sm.customerLeaveCount}";

        if (wrongDrinkText)
            wrongDrinkText.text = $"잘못된 음료 : {sm.wrongDrinkCount}";

        if (panelRoot)
            panelRoot.SetActive(true);

        BGMManager.Instance.PlayEnding();

        // 게임 일시정지
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 점수에 따라 등급 문자열 반환
    /// </summary>
    string GetGrade(int score)
    {
        if (score >= 90) return "S";
        if (score >= 80) return "A";
        if (score >= 70) return "B";
        return "C";
    }

    /// <summary>
    /// 새로 시작하기 버튼
    /// </summary>
    void OnClickRestart()
    {
        Time.timeScale = 1f;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        
        if (!string.IsNullOrEmpty(startSceneName))
            SceneManager.LoadScene(startSceneName);
        else
            Debug.LogWarning("[EndingResultUI] startSceneName이 비어있습니다.");
    }
    

    /// <summary>
    /// 이어하기 버튼
    /// </summary>
    void OnClickContinue()
    {
        if (panelRoot)
            panelRoot.SetActive(false);

        Time.timeScale = 1f;

        BGMManager.Instance.PlayGame();
    }
}
