using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RecipePage
{
    public string pageId;   //"basic_1" 같은 ID
    public GameObject root; //이 페이지 전체 오브젝트
    public Button pageButton;   //페이지 클릭 버튼
    public GameObject lockOverlay;  //잠금 아이콘/패널
    public Text priceText;  //해금 가격 표시
    public int unlockCost = 0;  //필요한 돈
    public bool unlockedByDefault;  //처음부터 열려있는지 여부

    [HideInInspector] public bool isUnlocked;
}
