using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; }

    [Header("Mixer")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] string bgmParam = "BGM_Volume";
    [SerializeField] string sfxParam = "SFX_Volume";

    const string KEY_BGM = "vol.bgm";
    const string KEY_SFX = "vol.sfx";

    [Header("BGM Clips")]
    [SerializeField] AudioClip gameBGM;

    AudioSource bgmSource;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = GetComponent<AudioSource>();
        if (bgmSource == null)
            bgmSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;  // 브금이니까 반복 재생

        float bgm = PlayerPrefs.GetFloat(KEY_BGM, 0.8f);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, 0.8f);
        SetBgm01(bgm, save: false);
        SetSfx01(sfx, save: false);

    }


    public void SetBgm01(float v, bool save = true)
    {
        mixer.SetFloat(bgmParam, Linear01ToDb(v));
        if (save)
            PlayerPrefs.SetFloat(KEY_BGM, v);
    }

    public void SetSfx01(float v, bool save = true)
    {
        mixer.SetFloat(sfxParam, Linear01ToDb(v));
        if (save)
            PlayerPrefs.SetFloat(KEY_SFX, v);
    }

    public float GetBgm01() => PlayerPrefs.GetFloat(KEY_BGM, 0.8f);
    public float GetSfx01() => PlayerPrefs.GetFloat(KEY_SFX, 0.8f);

    static float Linear01ToDb(float v)
    {
        if (v <= 0.0001f) 
            return -80f;

        return Mathf.Log10(v) * 20f; 
    }
    public void PlayGameBGM()
    {
        PlayBGM(gameBGM);
    }

    void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] 재생할 BGM 클립이 설정되지 않았습니다.");
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return; // 이미 같은 곡 재생 중이면 다시 안 틀어도 됨

        bgmSource.clip = clip;
        bgmSource.Play();
    }
}
