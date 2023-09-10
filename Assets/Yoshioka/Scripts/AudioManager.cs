using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public AudioSource BGM_audioSource;
    public AudioSource SE_audioSource;

    [SerializeField]
    private AudioClip[] BGMList;

    [SerializeField]
    private AudioClip[] SEList;

    public void PlayBGM(string name)
    {
        AudioClip audio = Array.Find(BGMList,BGM => BGM.name == name);

        if(audio == null)
        {
            Debug.Log(name + "というBGMは存在しません。");
            return;
        }

        BGM_audioSource.clip = audio;
        BGM_audioSource.Play();
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
    }
}
