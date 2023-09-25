using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public AudioMixer audioMixer;
    public AudioSource BGM_audioSource;
    public AudioSource BGM_audioSource_2;
    public AudioSource SE_audioSource;

    private AudioClip intro_BGM;
    private AudioClip loop_BGM;

    private int introSamples;
    private int loopSamples;

    [SerializeField]
    private AudioClip[] BGMList;

    [SerializeField]
    private AudioClip[] SEList;

    void Start()
    {
        if(!PlayerPrefs.HasKey("volume_BGM"))
        {
            PlayerPrefs.SetFloat("volume_BGM",2);
            PlayerPrefs.SetFloat("volume_SE",2);
            PlayerPrefs.Save();
        }

        audioMixer.SetFloat("BGM",ValueToVolume(PlayerPrefs.GetFloat("volume_BGM")));
        audioMixer.SetFloat("SE",ValueToVolume(PlayerPrefs.GetFloat("volume_SE")));
    }

    public float ValueToVolume(float value)
    {
        //5段階補正
        value = value*7-6;
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value*0.01f) * 20f,-80f,0f);
        return volume;
    }

    public void PlayBGM(string name, float delay = 0.0f)
    {
        AudioClip audio = Array.Find(BGMList,BGM => BGM.name == name);

        if(audio == null)
        {
            Debug.Log(name + "というBGMは存在しません。");
            return;
        }

        BGM_audioSource.clip = audio;
        BGM_audioSource.PlayDelayed(delay);
    }

    public void PlayBGM_FromIntroToLoop(string intro_name, string loop_name)
    {
        AudioClip _intro_BGM = Array.Find(BGMList,BGM => BGM.name == intro_name);
        AudioClip _loop_BGM = Array.Find(BGMList,BGM => BGM.name == loop_name);

        if(_intro_BGM == null)
        {
            Debug.Log(intro_name + "というBGMは存在しません。");
            return;
        }

        if(_loop_BGM == null)
        {
            Debug.Log(loop_name + "というBGMは存在しません。");
            return;
        }

        introSamples = _intro_BGM.samples;
        loopSamples = _loop_BGM.samples;

        intro_BGM = _intro_BGM;
        loop_BGM = _loop_BGM;

        BGM_audioSource.clip = intro_BGM;
        BGM_audioSource_2.clip = loop_BGM;
        BGM_audioSource.PlayScheduled(AudioSettings.dspTime);
        BGM_audioSource_2.PlayScheduled(AudioSettings.dspTime + ((float)BGM_audioSource.clip.samples / (float)BGM_audioSource.clip.frequency));
    }

    public void PlaySE(string name)
    {
        AudioClip audio = Array.Find(SEList,SE => SE.name == name);

        if(audio == null)
        {
            Debug.Log(name + "というSEは存在しません。");
            return;
        }

        SE_audioSource.PlayOneShot(audio);
    }

    public void StopBGM()
    {
        BGM_audioSource.Stop();
        BGM_audioSource_2.Stop();
    }
}
