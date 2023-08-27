using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip[] Sounds;
    public AudioClip[] BGM;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(int id)
    {
        source.PlayOneShot(Sounds[id]);
    }

    public void PlayBGM(int id)
    {
        source.clip = BGM[id];
        source.Play();
    }
}
