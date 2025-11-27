using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 재료 보유량
    private Dictionary<PurchaseObjectSO, int> materials = new();

    public int GetMaterialAmount(PurchaseObjectSO material)
    {
        return materials.ContainsKey(material) ? materials[material] : 0;
    }

    public void AddMaterial(PurchaseObjectSO material, int amount)
    {
        if (!materials.ContainsKey(material))
        {
            materials[material] = 0;
        }

        materials[material] += amount;
    }

    // TestCode
    public void TestToShowList()
    {
        foreach (var material in materials)
        {
            Debug.Log($"name : {material.Key}, quantity : {material.Value}");
        }
    }
}
