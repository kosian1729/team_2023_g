using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator canvasAnim;
    public GameObject optionPanel;
    public CanvasGroup versionInfoPanel;
    private bool isOpenVersionPanel;

    public GameObject CongraturationPanel;

    void Start()
    {
        //BGM再生中でなければ
        if(!AudioManager.Instance.BGM_audioSource.isPlaying)
        {
           AudioManager.Instance.PlayBGM_FromIntroToLoop("BGM深捜頭","BGM深捜ループ"); 
        }

        if(PlayerDataManager.Instance.GetFlag("isClear_Story5"))
        {
            CongraturationPanel.SetActive(true);
        }
        else
        {
            CongraturationPanel.SetActive(false);
        }
    }

    public void Click_Story()
    {
        canvasAnim.SetTrigger("SelectStory");
    }

    public void Cancel_StoryPanel()
    {
        canvasAnim.SetTrigger("CancelStory");
    }

    public void Click_Option()
    {
        optionPanel.SetActive(true);
    }

    public void Cancel_Option()
    {
        optionPanel.SetActive(false);
    }

    public void Click_Play(string name)
    {
        AudioManager.Instance.StopBGM();
        LoadingManager.Instance.LoadScene(name,2.0f);
    }

    public void Click_Learn()
    {
        LoadingManager.Instance.LoadScene("Learn",1.0f);
    }

    public void Click_Version()
    {
        //判定反転
        isOpenVersionPanel = !isOpenVersionPanel;

        if(isOpenVersionPanel)
        {
            versionInfoPanel.alpha = 1;
            versionInfoPanel.blocksRaycasts = true;
        }
        else
        {
            versionInfoPanel.alpha = 0;
            versionInfoPanel.blocksRaycasts = false;
        }
    }

    //Debug
    public void Click_Reset()
    {
        PlayerDataManager.Instance.Reset();
        SceneManager.LoadScene("Title");
    }
}
