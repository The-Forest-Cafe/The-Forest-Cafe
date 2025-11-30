using UnityEngine;
using UnityEngine.UI;

public class BookPager : MonoBehaviour
{
    [Header("Pages")]
    public GameObject[] spreads;

    [Header("Navigation")]
    public Button prevButton;
    public Button nextButton;

    int currentIndex = 0; 
    
    void Start()
    {
        ShowPage(0);

        prevButton.onClick.AddListener(OnPrev);
        nextButton.onClick.AddListener(OnNext);
    }

    void ShowPage(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, spreads.Length - 1);

        //모든 페이지 비활성
        for (int i = 0; i < spreads.Length; i++)
            spreads[i].SetActive(i == currentIndex);

        //버튼 상태 업데이트
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < spreads.Length - 1;
    }

    void OnPrev()
    {
        SFXManager.Instance.Play("book");
        ShowPage(currentIndex - 1);
    }

    void OnNext()
    {
        SFXManager.Instance.Play("book");
        ShowPage(currentIndex + 1);
    }
}
