using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] GameObject dialogRoot; //패널 전체 (대사창)
    [SerializeField] Text dialogText;       //실제 대사 텍스트
    [SerializeField] Image cursorImage;     //커서(화살표 등)

    [Header("Buttons")]
    [SerializeField] Button nextButton;     // 일반 → 다음 대사 버튼
    [SerializeField] Button closeButton;    // 마지막 줄 → 닫기 버튼

    [Header("배경 이미지")]
    [SerializeField] Image dialogBackgroundImage;   // Panel_Dialog의 Image
    [SerializeField] Sprite defaultDialogSprite;    // 기본 대화창 이미지

    [Header("Cursor Motion")]
    [SerializeField] float cursorMoveAmplitude = 8f; //위아래 움직이는 범위
    [SerializeField] float cursorMoveSpeed = 6f;     //속도

    List<string> lines = new List<string>();
    int currentIndex = 0;
    bool isShowing = false;
    float cursorBaseX;

    public bool IsShowing => isShowing;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dialogRoot != null)
            dialogRoot.SetActive(false);

        if (cursorImage != null)
            cursorBaseX = cursorImage.rectTransform.anchoredPosition.x;

        // 버튼 리스너 설정
        if (nextButton != null)
            nextButton.onClick.AddListener(NextLine);

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    void Update()
    {
        if(!isShowing)
            return;

        //화면 아무데나 클릭 시 다음 대사
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }

        //커서 좌우로 살짝 움직이게
        if (cursorImage != null)
        {
            var rt = cursorImage.rectTransform;
            var pos = rt.anchoredPosition;
            pos.x = cursorBaseX + Mathf.Sin(Time.unscaledTime * cursorMoveSpeed) * cursorMoveAmplitude;
            rt.anchoredPosition = pos;
        }
    }

    public void Show(string[] newLines, Sprite bgSprite = null)
    {
        if (newLines == null || newLines.Length == 0)
        {
            Debug.LogWarning("DialogManager.Show : lines 가 비어있음");
            return;
        }

        // 배경 스프라이트 교체
        if (dialogBackgroundImage != null)
        {
            if (bgSprite != null)
                dialogBackgroundImage.sprite = bgSprite;
            else
                dialogBackgroundImage.sprite = defaultDialogSprite;
        }

        lines.Clear();
        lines.AddRange(newLines);
        currentIndex = 0;
        isShowing = true;

        if (dialogRoot != null)
            dialogRoot.SetActive(true);

        ApplyCurrentLine();
        UpdateButtons();
    }

    void ApplyCurrentLine()
    {
        if (currentIndex >= 0 && currentIndex < lines.Count)
        {
            if (dialogText != null)
                dialogText.text = lines[currentIndex];
        }
        UpdateButtons();
    }

    void UpdateButtons()
    {
        // 마지막 대사인지 체크
        bool isLast = (currentIndex == lines.Count - 1);

        if (nextButton != null)
            nextButton.gameObject.SetActive(!isLast);  // 마지막 줄이면 숨김

        if (closeButton != null)
            closeButton.gameObject.SetActive(isLast);  // 마지막 줄이면 보임
    }

    void NextLine()
    {
        currentIndex++;
        if (currentIndex >= lines.Count)
        {
            Close();
        }
        else
        {
            ApplyCurrentLine();
        }
    }

    public void Close()
    {
        isShowing = false;

        if (dialogRoot != null)
            dialogRoot.SetActive(false);
    }
}
