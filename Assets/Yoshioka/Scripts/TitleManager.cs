using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Animator startTextAnim;
    private float timer;
    private bool isClick;

    void Start()
    {
        AudioManager.Instance.PlayBGM_FromIntroToLoop("BGM深捜頭","BGM深捜ループ");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlaySE("SEセレクト");

            startTextAnim.SetTrigger("LoadScene");
            isClick = true;
            timer = 0;
        }

        if(isClick)
        {
            timer+=Time.deltaTime;
        }

        if(timer>0.3f)
        {
            SceneManager.LoadScene("Menu");
        }

    }
}
