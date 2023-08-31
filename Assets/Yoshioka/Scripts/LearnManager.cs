using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LearnManager : MonoBehaviour
{
    public GameObject CheckScreenPanel;
    public PlayerController playerController;

    public void Click_BackMenu()
    {
        CheckScreenPanel.SetActive(true);
        playerController.enabled = false;
    }

    public void Click_CheckScreen_Yes()
    {
        playerController.enabled = true;
        SceneManager.LoadScene("Menu");
    }

    public void Click_CheckScreen_No()
    {
        playerController.enabled = true;
        CheckScreenPanel.SetActive(false);
    }
}
