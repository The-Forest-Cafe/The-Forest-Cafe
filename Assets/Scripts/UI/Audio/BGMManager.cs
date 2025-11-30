using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioClip gameBGM;
    public AudioClip endingBGM;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void Play(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void PlayGame()
        => Play(gameBGM, 0.5f);

    public void PlayEnding()
        => Play(endingBGM, 0.5f);
}
