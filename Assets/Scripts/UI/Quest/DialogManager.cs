using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("UI")]
    public GameObject dialogPanel;
    public Text dialogText;
    public Text nameText;

    string[] currentLines;
    int currentIndex;

    bool isTyping;
    Coroutine typingRoutine;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        //패널이 꺼져 있으면 입력 무시
        if (dialogPanel == null || !dialogPanel.activeSelf) return;

        //마우스 왼쪽 클릭시
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopTyping();
            }
            else
            {
                NextLine();
            }
        }
    }

    //외부에서 대사 시작할 때 호출
    public void StartDialog(string[] lines, string speakerName = "")
    {
        if (lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentIndex = 0;

        if (nameText != null)
            nameText.text = speakerName;

        if (dialogPanel != null)
            dialogPanel.SetActive(true);

        ShowLine();
    }

    void ShowLine()
    {
        if (currentLines == null || currentIndex >= currentLines.Length)
        {
            EndDialog();
            return;
        }

        string line = currentLines[currentIndex];

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        // 타이핑 효과 켜기
        typingRoutine = StartCoroutine(Typing(line));
    }

    IEnumerator Typing(string line)
    {
        isTyping = true;

        if (dialogText != null)
            dialogText.text = "";

        // typingSpeed가 0이거나 음수면 그냥 한 번에 출력
        if (typingSpeed <= 0f)
        {
            if (dialogText != null)
                dialogText.text = line;

            isTyping = false;
            yield break;
        }

        foreach (char c in line)
        {
            if (dialogText != null)
                dialogText.text += c;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void StopTyping()
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        if (dialogText != null && currentLines != null && currentIndex < currentLines.Length)
            dialogText.text = currentLines[currentIndex];

        isTyping = false;
    }

    void NextLine()
    {
        currentIndex++;
        ShowLine();
    }

    void EndDialog()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);

        currentLines = null;
        currentIndex = 0;
        isTyping = false;
    }
}
