using UnityEngine;

public class StartSceneController : MonoBehaviour
{
    void Start()
    {
        BGMManager.Instance.PlayGame();
    }
}
