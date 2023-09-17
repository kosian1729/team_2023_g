using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Story1Manager : MonoBehaviour
{
    public CameraScroll cameraScroll;
    public PlayerController playerController;
    public GameEvent BeforeStageStart;
    public GameObject playerInfo;   //UI
    public CanvasGroup blackScreen;  //暗転用
    public DialogueManager dialogueManager;
    private Color screenColor;

    void Start()
    {
        //blackScreenを透明にする
        blackScreen.alpha = 0;
        blackScreen.interactable = false;
        //Cameraのスクロールを止める
        cameraScroll.PouseCameraScroll(true);

        //Playerの操作を不可にする
        playerController.StopControll(true);

        playerInfo.SetActive(false);

        //Dialogueパート開始
        dialogueManager.StartDialogue(0);

        //Dialogueパートが終了時に呼び出される
        dialogueManager.OnEndLog = () =>{
            StartCoroutine(PhaseA());
        };
    }

    IEnumerator PhaseA()
    {
        yield return new WaitForSeconds(0.6f);

        AudioManager.Instance.PlayBGM_FromIntroToLoop("BGM調査頭","BGM調査ループ");

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
     
    public void Boss()
    {
        playerController.StopControll(true);
       
        cameraScroll.PouseCameraScroll(true);

        dialogueManager.StartDialogue(1);

        dialogueManager.OnEndLog = () =>{
            StartCoroutine(PhaseB());
        };
    }

    IEnumerator PhaseB()
    {
        yield return new WaitForSeconds(0.6f);
    }

    public void GameOver()
    {        
        cameraScroll.StartCoroutine("FadeOut");

        AudioManager.Instance.PlayBGM("BGMゲームオーバー",0.5f);

        blackScreen.DOFade(0.8f,2.0f);
        blackScreen.interactable = true;
    }

    public void Click_Retry()
    {
        LoadingManager.Instance.LoadScene("Story1",2.0f);
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
