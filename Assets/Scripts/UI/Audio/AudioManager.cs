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


    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

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
}
