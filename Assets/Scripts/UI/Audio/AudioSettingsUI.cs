using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void OnEnable()
    {
        //현재 저장값을 UI에 반영
        if(AudioManager.I != null)
        {
            bgmSlider.SetValueWithoutNotify(AudioManager.I.GetBgm01());
            sfxSlider.SetValueWithoutNotify(AudioManager.I.GetSfx01());
        }

        //리스너 연결
        bgmSlider.onValueChanged.AddListener(OnBgmChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);

    }

    private void OnDisable()
    {
        bgmSlider.onValueChanged.RemoveListener(OnBgmChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
        PlayerPrefs.Save();
    }
    void OnBgmChanged(float v) => AudioManager.I?.SetBgm01(v);
    void OnSfxChanged(float v) => AudioManager.I?.SetSfx01(v);
}
