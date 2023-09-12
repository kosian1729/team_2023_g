using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckAudioSample : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClip;

    void Start()
    {
        for(int i=0; i<audioClip.Length; i++)
        {
            Debug.Log($"{audioClip[i].name}のサンプル数は{audioClip[i].samples}");
        }
    }
}
