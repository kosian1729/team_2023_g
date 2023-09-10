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
        AudioManager.Instance.PlayBGM("BGM深捜ループ");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            startTextAnim.SetTrigger("LoadScene");
            isEnter = true;
            timer = 0;
        }

        if(isEnter)
        {
            timer+=Time.deltaTime;
        }

        if(timer>1.5f)
        {
            SceneManager.LoadScene("Menu");
        }

    }
}
