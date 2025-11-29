using UnityEngine;

public class FinalRecipeUI : MonoBehaviour
{
    [Header("조각 상태별 이미지 오브젝트 (0~4)")]
    //index 0 = 모두 찢어진 상태, index 4 = 완성
    public GameObject[] pieceStates;

    void OnEnable()
    {
        if (RecipeManager.Instance != null)
        {
            RecipeManager.Instance.OnFinalPiecesChanged += OnPiecesChanged;
            OnPiecesChanged(RecipeManager.Instance.finalRecipePieces);
        }
    }

    void OnDisable()
    {
        if (RecipeManager.Instance != null)
        {
            RecipeManager.Instance.OnFinalPiecesChanged -= OnPiecesChanged;
        }
    }

    void OnPiecesChanged(int pieceCount)
    {
        pieceCount = Mathf.Clamp(pieceCount, 0, pieceStates.Length - 1);

        for (int i = 0; i < pieceStates.Length; i++)
        {
            if (pieceStates[i] != null)
                pieceStates[i].SetActive(i == pieceCount);
        }
    }
}
