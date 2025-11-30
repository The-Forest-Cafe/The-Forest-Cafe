using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    AudioSource audioSource;

    [System.Serializable]
    public class SFXData
    {
        public string key;
        public AudioClip clip;
    }

    public List<SFXData> sfxList;

    Dictionary<string, AudioClip> sfxMap = new Dictionary<string, AudioClip>();

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

        foreach (var sfx in sfxList)
        {
            sfxMap[sfx.key] = sfx.clip;
        }
    }


    public void Play(string key)
    {
        if (sfxMap.ContainsKey(key))
            audioSource.PlayOneShot(sfxMap[key]);
    }

}
