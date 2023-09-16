using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public CameraScroll cameraScroll;
    public PlayerController playerController;
    public GameEvent BeforeStageStart;
    public GameObject playerInfo;   //UI
    public CanvasGroup blackScreen;  //暗転用
    private Color screenColor;

    IEnumerator Start()
    {
        AudioManager.Instance.PlayBGM_FromIntroToLoop("BGM調査頭","BGM調査ループ");

        //blackScreenを透明にする
        blackScreen.alpha = 0;
        //Cameraのスクロールを止める
        cameraScroll.PouseCameraScroll(true);

        //Playerの操作を不可にする
        playerController.StopControll(true);

        playerInfo.SetActive(false);

        //BeforeStageStartをRistenしているものを実行する
        BeforeStageStart.Raise();

        yield return new WaitForSeconds(3.8f);

        StartStage();
    }

    public void StartStage()
    {
        cameraScroll.PouseCameraScroll(false);
        cameraScroll.StartCoroutine("FadeIn",1.5f);
        playerController.StopControll(false);
        playerInfo.SetActive(true);
    }

    public void GameOver()
    {        
        cameraScroll.StartCoroutine("FadeOut");

        AudioManager.Instance.PlayBGM("BGMゲームオーバー",0.5f);

        blackScreen.DOFade(0.8f,2.0f);
    }
     
    public void Boss()
    {
        playerController.StopControll(true);
       
        cameraScroll.PouseCameraScroll(true);
    }

    public void Click_Retry()
    {
        LoadingManager.Instance.LoadScene("Stage0",2.0f);
    }

    public void Click_Back()
    {
        LoadingManager.Instance.LoadScene("Title",2.0f);
    }


    IEnumerator BlackOut()
    {

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Title");
    }
}
