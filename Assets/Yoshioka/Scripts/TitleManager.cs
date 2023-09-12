using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Animator startTextAnim;
    private float timer;
    private bool isEnter;

    void Start()
    {
        AudioManager.Instance.PlayBGM_FromIntroToLoop("BGM深捜頭","BGM深捜ループ");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager.Instance.PlaySE("SEセレクト");

            startTextAnim.SetTrigger("LoadScene");
            isEnter = true;
            timer = 0;
        }

        if(isEnter)
        {
            timer+=Time.deltaTime;
        }

        if(timer>0.3f)
        {
            SceneManager.LoadScene("Menu");
        }

    }
}
