using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientUnlockManager : MonoBehaviour
{
    public static IngredientUnlockManager Instance { get; private set; }

    HashSet<string> unlockedIds = new HashSet<string>();    //현재 해금된 재료 ID들

    public event Action<string> OnIngredientUnlocked;   //특정 재료 ID가 해금되었을 때 알림

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 재료 ID를 해금 (이미 해금된 경우 무시)
    /// </summary>
    public void UnlockById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        if (unlockedIds.Contains(id))
            return;

        unlockedIds.Add(id);

        Debug.Log($"재료 해금: {id}");

        // 이 ID를 사용하는 슬롯들에게 알림
        OnIngredientUnlocked?.Invoke(id);
    }

    /// <summary>
    /// 특정 재료ID가 해금되었는지 여부
    /// </summary>
    public bool IsUnlocked(string id)
    {
        return unlockedIds.Contains(id);
    }
}
