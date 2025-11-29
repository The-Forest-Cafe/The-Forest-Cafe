using UnityEngine;

[CreateAssetMenu(fileName = "New PurchaseObject", menuName = "New PurchaseObjectSO")]
public class PurchaseObjectSO : ScriptableObject
{
    public Sprite objectSprite;
    public string objectName;
    public int quantity;
    public int price;
    public bool isSelected;
}
