using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] 
    private AudioMixer audioMixer;
    [SerializeField] 
    private Slider bgmSlider;
    [SerializeField] 
    private Slider seSlider;

    void Start()
    {
        //スライダーを動かした時の処理を登録
        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        seSlider.onValueChanged.AddListener(SetAudioMixerSE);

        if(!PlayerPrefs.HasKey("volume_BGM"))
        {
            PlayerPrefs.SetFloat("volume_BGM",2);
            PlayerPrefs.SetFloat("volume_SE",2);
            PlayerPrefs.Save();
        }

        audioMixer.SetFloat("BGM",ValueToVolume(PlayerPrefs.GetFloat("volume_BGM")));
        audioMixer.SetFloat("SE",ValueToVolume(PlayerPrefs.GetFloat("volume_SE")));
        bgmSlider.value = PlayerPrefs.GetFloat("volume_BGM");
        seSlider.value = PlayerPrefs.GetFloat("volume_SE");
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    }

    //BGM
    public void SetAudioMixerBGM(float value)
    {
        PlayerPrefs.SetFloat("volume_BGM",value);

        var volume = ValueToVolume(value);
        //audioMixerに代入
        audioMixer.SetFloat("BGM",volume);

        Debug.Log($"BGM:{volume}");
    }

    //SE
    public void SetAudioMixerSE(float value)
    {
        PlayerPrefs.SetFloat("volume_SE",value);

        var volume = ValueToVolume(value);
        //audioMixerに代入
        audioMixer.SetFloat("SE",volume);

        Debug.Log($"SE:{volume}");
    }

    public float ValueToVolume(float value)
    {
        //5段階補正
        value = value*7-6;
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value*0.01f) * 20f,-80f,0f);
        return volume;
    }
}
