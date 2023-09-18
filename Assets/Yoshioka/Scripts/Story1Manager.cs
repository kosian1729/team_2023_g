using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Story1Manager : MonoBehaviour
{
    public GameObject camera;
    private CameraScroll cameraScroll;
    public PlayerController playerController;
    public BossSeaDragonController bossController;
    public GameEvent BeforeStageStart;
    public GameObject playerInfo;   //UI
    public CanvasGroup blackScreen;  //暗転用
    public CanvasGroup bossInfo;
    public CanvasGroup result;
    public GameObject resultText;
    public DialogueManager dialogueManager;
    [Header("ボス戦のカメラ位置")]
    public Transform BossCameraPos;
    private Color screenColor;

    void Start()
    {
        cameraScroll = camera.GetComponent<CameraScroll>();
        //blackScreenを透明にする
        blackScreen.alpha = 0;
        blackScreen.interactable = false;
        blackScreen.blocksRaycasts = false;
        //bossInfoを透明にする
        bossInfo.alpha = 0;
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
     
    public void Boss()
    {
        playerController.StopControll(true);
       
        cameraScroll.PouseCameraScroll(true);

        AudioManager.Instance.StopBGM();

        dialogueManager.StartDialogue(1);

        dialogueManager.OnEndLog = () =>{

            var sequence = DOTween.Sequence();

            sequence.AppendInterval(0.6f)   //0-0.6
                    .Append(camera.transform.DOMove(BossCameraPos.position,12.0f)) //0.6-4.6
                    .Join(bossInfo.DOFade(1,0.4f));

            sequence.InsertCallback(0.6f, () =>
            {
                playerController.StopControll(false);
                bossController.BossStart();
                AudioManager.Instance.PlayBGM_FromIntroToLoop("BGMボス戦AGG頭","BGMボス戦AGGループ");
            });
        };
    }

    public void AfterBoss()
    {
        bossInfo.DOFade(0,0.4f);

        playerController.StopControll(true);
        AudioManager.Instance.StopBGM();

        dialogueManager.StartDialogue(2);

        dialogueManager.OnEndLog = () =>{

            AudioManager.Instance.PlayBGM_FromIntroToLoop("BGMリザルト頭","BGMリザルトループ");

            var sequence = DOTween.Sequence();

            sequence.Append(result.DOFade(1,0.2f))
                    .Join(resultText.transform.DOScale(new Vector3(1,1,1), 0.6f));
            
            result.interactable = true;
            result.blocksRaycasts = true;
        };
    }

    public void GameOver()
    {        
        cameraScroll.StartCoroutine("FadeOut");

        AudioManager.Instance.PlayBGM("BGMゲームオーバー",0.5f);

        blackScreen.DOFade(0.8f,2.0f);
        blackScreen.interactable = true;
        blackScreen.blocksRaycasts = true;
    }

    public void Click_Retry()
    {
        LoadingManager.Instance.LoadScene("Story1",2.0f);
    }

    public void Click_Back()
    {
        LoadingManager.Instance.LoadScene("Title",2.0f);
    }
}
