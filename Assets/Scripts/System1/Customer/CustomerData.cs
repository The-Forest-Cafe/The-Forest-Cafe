using UnityEngine;
using System.Collections.Generic;

public enum CustomerType { General, Special }

[CreateAssetMenu(fileName = "New Customer", menuName = "Cafe/Customer")]
public class CustomerData : ScriptableObject
{
    public string npcName;           
    public GameObject npcPrefab;  
    public CustomerType type;        

    [Header("주문 가능 메뉴")]
    public List<Recipe> possibleMenus;
}
