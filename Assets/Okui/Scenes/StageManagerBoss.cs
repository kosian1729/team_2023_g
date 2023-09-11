using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManagerBoss: MonoBehaviour
{
    public CameraScroll cameraScroll;
    public PlayerController playerController;
    public GameEvent BeforeStageStart;
    public GameObject playerInfo;   //UI
    public Image blackScreen;  //�Ó]�p
    private Color screenColor;

    IEnumerator Start()
    {
        AudioManager.Instance.PlayBGM("BGM�������[�v");

        screenColor = blackScreen.color;
        screenColor.a = 0;
        blackScreen.color = screenColor;
        //Camera�̃X�N���[�����~�߂�
        cameraScroll.PouseCameraScroll(true);

        //Player�̑����s�ɂ���
        playerController.StopControll(true);

        playerInfo.SetActive(false);

        //BeforeStageStart��Risten���Ă�����̂����s����
        BeforeStageStart.Raise();

        yield return new WaitForSeconds(3.8f);

        StartStage();
    }

    public void StartStage()
    {
        cameraScroll.PouseCameraScroll(false);
        cameraScroll.StartCoroutine("FadeIn", 1.5f);
        playerController.StopControll(false);
        playerInfo.SetActive(true);
    }

    public void GameOver()
    {
        //Player�̑����s�ɂ���
        playerController.StopControll(true);

        cameraScroll.StartCoroutine("FadeOut");


        StartCoroutine("BlackOut");
    }

    IEnumerator BlackOut()
    {
        for (int i = 0; i < 200; i++)
        {
            if (screenColor.a <= 1.0f)
            {
                screenColor.a += 0.02f;
                blackScreen.color = screenColor;
            }

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Title");
    }
}
