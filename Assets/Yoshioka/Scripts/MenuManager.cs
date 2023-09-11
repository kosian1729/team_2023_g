using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator canvasAnim;
    public GameObject optionPanel;

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
        SceneManager.LoadScene("Stage"+name);
    }

    public void Click_Learn()
    {
        SceneManager.LoadScene("Learn");
    }
}
