using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Story5Manager : MonoBehaviour
{
    public GameObject camera;
    private CameraScroll cameraScroll;
    public PlayerController playerController;
    public BossMachineController bossController;
    public GameEvent BeforeStageStart;
    public GameObject playerInfo;   //UI
    public CanvasGroup blackScreen;  //暗転用
    public GameObject easyRetry;    //Boss前復活用
    public CanvasGroup bossInfo;
    public CanvasGroup result;
    public GameObject resultText;
    public GameObject spawnArea;
    public GameObject despawnArea;
    public DialogueManager dialogueManager;
    [Header("ボス戦のカメラ位置")]
    public Transform BossCameraPos;
    [Header("ボス前リトライの位置")]
    public float x;
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
        
        if(PlayerDataManager.Instance.GetFlag("isBoss_Story5"))
        {
            camera.transform.position += new Vector3(x,0,0);
            StartCoroutine(PhaseA());
        }
        else
        {
            //Dialogueパート開始
            dialogueManager.StartDialogue(0);
            //Dialogueパートが終了時に呼び出される
            dialogueManager.OnEndLog = () =>{
                StartCoroutine(PhaseA());
            };
        }
    }

    IEnumerator PhaseA()
    {
        AudioManager.Instance.PlayBGM_FromIntroToLoop("BGM作戦頭","BGM作戦ループ");   
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
                    .Append(camera.transform.DOMove(BossCameraPos.position,8.0f)) //0.6-8.6
                    .Join(bossInfo.DOFade(1,0.4f));

            sequence.InsertCallback(0.6f, () =>
            {
                playerController.StopControll(false);
                AudioManager.Instance.PlayBGM_FromIntroToLoop("BGMボス戦TER頭","BGMボス戦TERループ");
            });

            sequence.InsertCallback(7.5f, () =>
            {
                spawnArea.SetActive(false);
                despawnArea.SetActive(false);
                bossController.enabled = true;
                bossController.BossStart();
            });
        };

        //Boss遭遇フラグ
        PlayerDataManager.Instance.SetFlag("isBoss_Story5",true);
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

        //Boss遭遇false　ストーリークリアtrue
        PlayerDataManager.Instance.SetFlag("isBoss_Story5",false);
        PlayerDataManager.Instance.SetFlag("isClear_Story5",true);
    }

    public void GameOver()
    {        
        cameraScroll.StartCoroutine("FadeOut");

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("BGMゲームオーバー",0.5f);

        //Bossにたどり着いていたら選択肢に表示
        if(PlayerDataManager.Instance.GetFlag("isBoss_Story5"))
        {
            easyRetry.SetActive(true);
        }
        else
        {
            easyRetry.SetActive(false);
        }
        
        blackScreen.DOFade(0.8f,2.0f);
        blackScreen.interactable = true;
        blackScreen.blocksRaycasts = true;
    }

    public void Click_Retry()
    {
        PlayerDataManager.Instance.SetFlag("isBoss_Story5",false);
        LoadingManager.Instance.LoadScene("Story5",2.0f);
    }

    public void Click_Back()
    {
        PlayerDataManager.Instance.SetFlag("isBoss_Story5",false);
        LoadingManager.Instance.LoadScene("Title",3.0f);
    }

    public void Click_EasyRetry()
    {
        LoadingManager.Instance.LoadScene("Story5",1.9f);
    }
}
