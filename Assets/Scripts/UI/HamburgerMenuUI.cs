using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HamburgerMenuUI : MonoBehaviour
{
    public Button menuButton;
    public Image menuButtonImage;
    public GameObject subMenu;
    

    [Header("메뉴 아이콘")]
    [SerializeField] Sprite hamburgerIcon;
    [SerializeField] Sprite closeIcon;

    [Header("하위 메뉴 버튼")]
    [SerializeField] Button questButton;
    [SerializeField] Button recipeButton;
    [SerializeField] Button settingsButton;

    public bool IsExpanded { get; private set; }

    void Awake()
    {
        SetExpanded(false, instant: true);

        if (menuButton)
            menuButton.onClick.AddListener(Toggle);
        
    }

    void Update()
    {
        if (IsExpanded && Input.GetKeyDown(KeyCode.Escape))
        {
            SetExpanded(false);
        }
    }

    public void Toggle()
    {
        SetExpanded(!IsExpanded);
    }

    public void SetExpanded(bool expand, bool instant = false)
    {
        IsExpanded = expand;

        //메뉴 아이콘 교체
        if(menuButtonImage)
            menuButtonImage.sprite = expand ? closeIcon : hamburgerIcon;

        //서브메뉴 표시
        if(subMenu)
            subMenu.SetActive(expand);
        
        if (questButton) questButton.onClick.AddListener(OnClickQuest);
        if (recipeButton) recipeButton.onClick.AddListener(OnClickRecipe);
        if (settingsButton) settingsButton.onClick.AddListener(OnClickSettings);

    }

    void OnClickQuest()
    {
        Debug.Log("QuestBook Open");
        SetExpanded(false);
        //TODO: 퀘스트북 UI 열기 로직 호출
    }

    void OnClickRecipe()
    {
        Debug.Log("QuestBook Open");
        SetExpanded(false);
        //TODO: 레시피북 UI 열기 로직 호출
    }

    void OnClickSettings()
    {
        Debug.Log("QuestBook Open");
        SetExpanded(false);
        //TODO: 설정창 UI 열기 로직 호출
    }
}
