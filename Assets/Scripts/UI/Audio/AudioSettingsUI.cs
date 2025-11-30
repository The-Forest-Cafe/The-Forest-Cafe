using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Button okButton;
    [SerializeField] Button cancelButton;

    // 패널 열기 전 값 저장용
    float originalBgm;
    float originalSfx;

    private void OnEnable()
    {
        //현재 저장값 불러오기
        originalBgm = AudioManager.Instance.GetBgm01();
        originalSfx = AudioManager.Instance.GetSfx01();

        //UI에 반영
        bgmSlider.SetValueWithoutNotify(originalBgm);
        sfxSlider.SetValueWithoutNotify(originalSfx);

        //슬라이더는 미리바뀌는 소리 재생 안함 (실시간 반영 X)
        bgmSlider.onValueChanged.AddListener(OnBgmChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);

        okButton.onClick.AddListener(OnClickOK);
        cancelButton.onClick.AddListener(OnClickCancel);

    }

    private void OnDisable()
    {
        bgmSlider.onValueChanged.RemoveListener(OnBgmChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

        okButton.onClick.RemoveListener(OnClickOK);
        cancelButton.onClick.RemoveListener(OnClickCancel);
    }

    //슬라이더 움직일 때: 소리만 바뀜(저장 X)
    void OnBgmChanged(float v)
    {
        AudioManager.Instance.SetBgm01(v, save: false);
    }

    void OnSfxChanged(float v)
    {
        AudioManager.Instance.SetSfx01(v, save: false);
    }

    //확인 버튼 눌렀을 때: 진짜 저장
    void OnClickOK()
    {
        float bgm = bgmSlider.value;
        float sfx = sfxSlider.value;

        AudioManager.Instance.SetBgm01(bgm, save: true);
        AudioManager.Instance.SetSfx01(sfx, save: true);

        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }

    //취소 버튼: 원래 값으로 되돌리기
    void OnClickCancel()
    {
        AudioManager.Instance.SetBgm01(originalBgm, save: false);
        AudioManager.Instance.SetSfx01(originalSfx, save: false);

        //UI도 초기값으로 되돌림
        bgmSlider.SetValueWithoutNotify(originalBgm);
        sfxSlider.SetValueWithoutNotify(originalSfx);

        gameObject.SetActive(false);
    }
}
