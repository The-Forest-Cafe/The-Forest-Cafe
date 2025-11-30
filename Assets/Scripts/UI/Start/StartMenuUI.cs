using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    [Header("시작할 게임 씬 이름")]
    [SerializeField] string gameSceneName = "MainScene";
    //실제 게임 씬 이름으로 변경해서 사용하기

    //시작 버튼에서 호출
    public void OnClickStart()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("gameSceneName이 비어 있습니다. 인스펙터에서 설정해주세요.");
        }
    }

    //나가기 버튼에서 호출
    public void OnClickQuit()
    {
        Debug.Log("게임 종료");

        Application.Quit();

#if UNITY_EDITOR
        // 에디터에서 테스트할 때는 이 코드가 실행되어 플레이 모드만 종료됨
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
