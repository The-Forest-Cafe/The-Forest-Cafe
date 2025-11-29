using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField]
    private Image checkImage;
    private bool isChecked;

    public delegate void OnObjectSelected();
    public event OnObjectSelected onObjectSelected;

    private void Start()
    {
        InitCheckBtn();

        PurchaseManager.Instance.onPurchaseListInited += InitCheckBtn;
    }

    private void InitCheckBtn()
    {
        isChecked = false;
        checkImage.gameObject.SetActive(isChecked);
    }

    public void OnCheckBtnClicked()
    {
        isChecked = !isChecked;

        checkImage.gameObject.SetActive(isChecked);

        onObjectSelected?.Invoke();
    }

    public bool GetIsSelected()
    {
        return isChecked;
    }
}
