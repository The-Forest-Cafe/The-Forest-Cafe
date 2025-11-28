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

    [Header("Cursor Motion")]
    [SerializeField] float cursorMoveAmplitude = 8f; //위아래 움직이는 범위
    [SerializeField] float cursorMoveSpeed = 6f;     //속도

    List<string> lines = new List<string>();
    int currentIndex = 0;
    bool isShowing = false;
    float cursorBaseY;

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
            cursorBaseY = cursorImage.rectTransform.anchoredPosition.y;
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

        //커서 위아래로 살짝 움직이게
        if (cursorImage != null)
        {
            var rt = cursorImage.rectTransform;
            var pos = rt.anchoredPosition;
            pos.y = cursorBaseY + Mathf.Sin(Time.unscaledTime * cursorMoveSpeed) * cursorMoveAmplitude;
            rt.anchoredPosition = pos;
        }
    }

    public void Show(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0)
        {
            Debug.LogWarning("DialogManager.Show : lines 가 비어있음");
            return;
        }

        lines.Clear();
        lines.AddRange(newLines);
        currentIndex = 0;
        isShowing = true;

        if (dialogRoot != null)
            dialogRoot.SetActive(true);

        ApplyCurrentLine();
    }

    void ApplyCurrentLine()
    {
        if (currentIndex >= 0 && currentIndex < lines.Count)
        {
            if (dialogText != null)
                dialogText.text = lines[currentIndex];
        }
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
