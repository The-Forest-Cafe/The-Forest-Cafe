using UnityEngine;
using UnityEngine.UI;
using System;

public class ClockTimerUI : MonoBehaviour
{
    [Header("시계 채우기 이미지")]
    [SerializeField] Image fillImage;   //Filled로 설정된 이미지

    [Header("자동 숨김 옵션")]
    [SerializeField] bool hideWhenDone = true;   //끝나면 자동으로 끄기

    float duration;       //전체 시간
    float elapsed;        //경과 시간
    bool isRunning;

    //타이머 완료시 알려주고 싶으면 사용
    public Action OnTimerCompleted;

    void Awake()
    {
        if (fillImage != null)
            fillImage.fillAmount = 0f;

        gameObject.SetActive(false); //처음엔 안보이게
    }

    void Update()
    {
        if (!isRunning) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        if (fillImage != null)
            fillImage.fillAmount = t;

        if (t >= 1f)
        {
            isRunning = false;

            if (hideWhenDone)
                gameObject.SetActive(false);

            OnTimerCompleted?.Invoke();
        }
    }

    /// <summary>
    /// duration 초 동안 시계를 채우는 타이머 시작
    /// </summary>
    public void StartTimer(float durationSeconds)
    {
        if (durationSeconds <= 0f)
        {
            // 0초면 바로 완료 처리
            duration = 0.01f;
        }
        else
        {
            duration = durationSeconds;
        }

        elapsed = 0f;
        isRunning = true;
        gameObject.SetActive(true);

        if (fillImage != null)
            fillImage.fillAmount = 0f;
    }

    /// <summary>
    /// 강제로 중지하고 숨기기
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;
        if (hideWhenDone)
            gameObject.SetActive(false);
    }
}
