using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public CameraScroll cameraScroll;
    public PlayerController playerController;
    public GameEvent BeforeStageStart;
    public GameObject playerInfo;   //UI

    IEnumerator Start()
    {
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
}
