using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Story4Manager : MonoBehaviour
{
    public GameObject camera;
    private CameraScroll cameraScroll;
    public PlayerController playerController;
    public GameEvent BeforeStageStart;
    public GameObject playerInfo;   //UI
    public CanvasGroup blackScreen;  //暗転用
    public CanvasGroup result;
    public GameObject resultText;
    public DialogueManager dialogueManager;
    private Color screenColor;

    void Start()
    {
        cameraScroll = camera.GetComponent<CameraScroll>();
        //blackScreenを透明にする
        blackScreen.alpha = 0;
        blackScreen.interactable = false;
        blackScreen.blocksRaycasts = false;
        //resultを透明にする
        result.alpha = 0;
        result.interactable = false;
        result.blocksRaycasts = false;
        resultText.transform.DOScale(new Vector3(0.1f,0.1f,0.1f),0.01f);
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
        AudioManager.Instance.PlayBGM_FromIntroToLoop("BGM調査頭","BGM調査ループ");   
        yield return new WaitForSeconds(0.6f);

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

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("BGMゲームオーバー",0.5f);
        
        blackScreen.DOFade(0.8f,2.0f);
        blackScreen.interactable = true;
        blackScreen.blocksRaycasts = true;
    }

    public void Clear()
    {
        playerController.StopControll(true);
        cameraScroll.PouseCameraScroll(true);
        
        AudioManager.Instance.StopBGM();

        //Dialogueパート開始
        dialogueManager.StartDialogue(1);
        
        //Dialogueパートが終了時に呼び出される
        dialogueManager.OnEndLog = () =>{
            AudioManager.Instance.PlayBGM_FromIntroToLoop("BGMリザルト頭","BGMリザルトループ");

            var sequence = DOTween.Sequence();

            sequence.Append(result.DOFade(1,0.2f))
                    .Join(resultText.transform.DOScale(new Vector3(1,1,1), 0.6f));
            
            result.interactable = true;
            result.blocksRaycasts = true;
        };

        PlayerDataManager.Instance.SetFlag("isClear_Story4",true);
    }

    public void Click_Retry()
    {
        LoadingManager.Instance.LoadScene("Story4",2.0f);
    }

    public void Click_Back()
    {
        LoadingManager.Instance.LoadScene("Title",3.0f);
    }
}
