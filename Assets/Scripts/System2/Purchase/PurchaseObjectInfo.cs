using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseObjectInfo : MonoBehaviour
{
    [SerializeField]
    private PurchaseObjectSO objectSO;
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private Text objectName;
    [SerializeField]
    private Text objectDisc;
    [SerializeField]
    private Text objectPrice;
    [SerializeField]
    private SelectButton checkButton;

    private void Start()
    {
        objectImage.sprite = objectSO.objectSprite;
        objectName.text = $"{objectSO.objectName} {objectSO.quantity}개";
        objectDisc.text = objectSO.objectDisc;
        objectPrice.text = $"{objectSO.price.ToString()}원";
        checkButton.onObjectSelected += SetObjectSelect;
    }

    // PurchaseObjectSO의 isSelected 변수의 값을 변경하는 함수
    private void SetObjectSelect()
    {
        objectSO.isSelected = checkButton.GetIsSelected();

        if (objectSO.isSelected == true)
        {
            TotalSum.Instance.totalSum += objectSO.price;
            PurchaseManager.Instance.AddList(objectSO);
        }
        else
        {
            TotalSum.Instance.totalSum -= objectSO.price;
            PurchaseManager.Instance.RemoveList(objectSO);
        }

        TotalSum.Instance.SetSumText();
    }
}
