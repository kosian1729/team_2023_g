using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject storyPanel;
    public GameObject optionPanel;

    public void Click_Story()
    {
        storyPanel.SetActive(true);
    }

    public void Cancel_StoryPanel()
    {
        storyPanel.SetActive(false);
    }

    public void Click_Option()
    {
        optionPanel.SetActive(true);
    }

    public void Cancel_Option()
    {
        optionPanel.SetActive(false);
    }

    public void Click_Play(int num)
    {
        AudioManager.Instance.StopBGM();
        SceneManager.LoadScene("Stage"+num);
    }

    public void Click_Learn()
    {
        SceneManager.LoadScene("Learn");
    }
}
