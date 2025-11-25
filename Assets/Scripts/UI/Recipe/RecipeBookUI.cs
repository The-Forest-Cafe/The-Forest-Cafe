using UnityEngine;
using UnityEngine.UI;

public class RecipeBookUI : MonoBehaviour
{
    public static RecipeBookUI Instance { get; private set; }

    [Header("Basic Recipe Page")]
    [SerializeField] RecipePage[] basicPages;

    [Header("Final Recipe")]
    [SerializeField] GameObject finalPageRoot;  //최종 레시피 전체
    [SerializeField] Image[] finalPieces;   //조각 4개 이미지
    [SerializeField] GameObject[] finalPieceLocks;  //각 조각 위 잠금/회색 처리 오브젝트

    [Header("Page Navigation")]
    [SerializeField] Button prevButton;
    [SerializeField] Button nextButton;
    [SerializeField] Text pageNumberText;

    int currentPageIndex = 0;
    int basicSpreadCount => Mathf.CeilToInt(basicPages.Length / 2f);
    int totalPages => basicSpreadCount + 1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //기본 레시피 페이지 초기화
        for(int i =0; i<basicPages.Length; i++)
        {
            var page = basicPages[i];

            //처음 잠금 상태
            page.isUnlocked = page.unlockedByDefault;
            UpdateBasicPageUI(i);

            //클릭 리스너 연결
            int pageIndex = i;
            if(page.pageButton != null)
            {
                page.pageButton.onClick.AddListener(() => OnClickBasicPage(pageIndex));
            }
        }

        //네비게이션 버튼 연결
        if (prevButton) prevButton.onClick.AddListener(PrevPage);
        if (nextButton) nextButton.onClick.AddListener(NextPage);

        //시작 페이지 표시
        ShowPage(0);

        //최종 레시피 초기 UI
        UpdateFinalRecipeUI();
    }

    //페이지 표시/넘기기
    void ShowPage(int index)
    {
        index = Mathf.Clamp(index, 0, totalPages - 1);
        currentPageIndex = index;

        //기본 페이지들 On/Off
        for(int i =0; i<basicPages.Length; i++)
        {
            if (basicPages[i].root != null)
            {
                int spreadIndex = i / 2; // 0: 1,2 / 1: 3,4 / ...
                bool active = (spreadIndex == currentPageIndex) && (currentPageIndex < basicSpreadCount);
                basicPages[i].root.SetActive(active);
            }
        }

        //최종 레시피 페이지 On/Off
        if(finalPageRoot != null)
        {
            bool isFinalPageSpread = (currentPageIndex == basicSpreadCount); // 마지막 장
            finalPageRoot.SetActive(isFinalPageSpread);
        }

        //이전/다음 버튼 활성 상태 조정
        if (prevButton) prevButton.interactable = (currentPageIndex > 0);
        if (nextButton) nextButton.interactable = (currentPageIndex < totalPages - 1);

        //페이지 번호 표시
        if (pageNumberText)
            pageNumberText.text = $"{currentPageIndex + 1} / {totalPages}";
    }

    public void NextPage()
    {
        ShowPage(currentPageIndex + 1);
    }

    public void PrevPage()
    {
        ShowPage(currentPageIndex - 1);
    }

    //기본 레시피 페이지
    void OnClickBasicPage(int index)
    {
        var page = basicPages[index];

        if (page.isUnlocked)
        {
            Debug.Log($"[RecipeBook] {page.pageId} 페이지 이미 해금됨.");
            //TODO: 필요하면 여기서 상세 팝업 열기 등
            return;
        }

        //잠금된 상태 -> 돈 사용해서 해금 시도
        if (page.unlockCost <= 0)
        {
            Debug.Log($"[RecipeBook] {page.pageId} 는 가격이 0이라 바로 해금.");
            UnlockBasicPage(index);
            return;
        }

        if (MoneyManager.Instance == null)
        {
            Debug.LogWarning("[RecipeBook] MoneyManager.Instance 없음!");
            return;
        }

        bool success = MoneyManager.Instance.SpendMoney(page.unlockCost);
        if (success)
        {
            UnlockBasicPage(index);
        }
        else
        {
            Debug.Log("[RecipeBook] 돈이 부족해서 레시피 해금 실패");
            //TODO: "돈이 부족합니다" UI 같은 거 띄워도 됨
        }
    }

    void UnlockBasicPage(int index)
    {
        var page = basicPages[index];
        page.isUnlocked = true;
        UpdateBasicPageUI(index);
        Debug.Log($"[RecipeBook] {page.pageId} 해금 완료!");
        //TODO: 해금 이펙트/사운드 추가 가능
    }

    void UpdateBasicPageUI(int index)
    {
        var page = basicPages[index];

        //잠금 오버레이 On/Off
        if (page.lockOverlay != null)
            page.lockOverlay.SetActive(!page.isUnlocked);

        //가격 텍스트 표시
        if (page.priceText != null)
        {
            if (page.isUnlocked || page.unlockCost <= 0)
                page.priceText.text = "";
            else
                page.priceText.text = page.unlockCost.ToString();
        }
    }

    //최종 레시피 조각
    //외부(퀘스트 성공 시)에서 호출:
    //RecipeBookUI.Instance.UnlockFinalPiece(pieceIndex);
    public void UnlockFinalPiece(int pieceIndex)
    {
        if (finalPieces == null || pieceIndex < 0 || pieceIndex >= finalPieces.Length)
        {
            Debug.LogWarning("[RecipeBook] 잘못된 조각 index");
            return;
        }

        if (finalPieceLocks != null && pieceIndex < finalPieceLocks.Length && finalPieceLocks[pieceIndex] != null)
        {
            finalPieceLocks[pieceIndex].SetActive(false);
        }

        Debug.Log($"[RecipeBook] 최종 레시피 조각 {pieceIndex} 해금!");
        //필요하면 여기서 "완성도" 체크해서 전체 완성 연출도 가능
    }

    void UpdateFinalRecipeUI()
    {
        //처음 상태: 잠금 오브젝트가 있다면 전부 켜두거나,
        //미리 에디터에서 조절해도 됨.
        if (finalPieceLocks != null)
        {
            for (int i = 0; i < finalPieceLocks.Length; i++)
            {
                if (finalPieceLocks[i] != null)
                    finalPieceLocks[i].SetActive(true); // 기본은 잠금 상태
            }
        }
    }
}
