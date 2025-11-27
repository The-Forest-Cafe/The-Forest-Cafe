using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    private Text checkText;
    private bool isChecked;

    public delegate void OnObjectSelected();
    public event OnObjectSelected onObjectSelected;

    private void Start()
    {
        checkText = GetComponentInChildren<Text>();
        InitCheckBtn();

        PurchaseManager.Instance.onPurchaseListInited += InitCheckBtn;
    }

    private void InitCheckBtn()
    {
        isChecked = false;
        checkText.gameObject.SetActive(isChecked);
    }

    public void OnCheckBtnClicked()
    {
        isChecked = !isChecked;

        checkText.gameObject.SetActive(isChecked);

        onObjectSelected?.Invoke();
    }

    public bool GetIsSelected()
    {
        return isChecked;
    }
}
