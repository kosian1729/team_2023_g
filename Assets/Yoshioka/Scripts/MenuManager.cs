using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject storyPanel;

    public void Click_Story()
    {
        storyPanel.SetActive(true);
    }

    public void Cancel_StoryPanel()
    {
        storyPanel.SetActive(false);
    }

    public void Click_Play(int num)
    {
        SceneManager.LoadScene("Stage"+num);
    }

    public void Click_Learn()
    {
        SceneManager.LoadScene("Learn");
    }
}
