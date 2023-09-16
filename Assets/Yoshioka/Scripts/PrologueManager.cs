using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Image backGround;
    public Sprite phaseA_sprite,phaseB_sprite;

    void Start()
    {
        backGround.DOFade(0,0);
        backGround.sprite = phaseA_sprite;
        backGround.DOFade(1,1.5f).SetDelay(0.5f).OnComplete(() =>
        {
            dialogueManager.StartDialogue(0);
            dialogueManager.OnEndLog += PhaseB;    
        });
    }

    void PhaseB()
    {
        dialogueManager.OnEndLog -= PhaseB;
        dialogueManager.OnEndLog += PhaseEnd;
        backGround.DOFade(0,0.2f).OnComplete(() => 
        {
            backGround.sprite = phaseB_sprite;
            backGround.DOFade(1,0.4f).SetDelay(1.5f).OnComplete(() =>
            {
                dialogueManager.StartDialogue(1);
            });
        });
    }

    void PhaseEnd()
    {
        dialogueManager.OnEndLog = null;

        backGround.DOFade(0,2.0f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            StartCoroutine(EndCoroutine());
        });
    }

    IEnumerator EndCoroutine()
    {
        //Flag
        PlayerDataManager.Instance.SetFlag("isClear_Prologue",true);
        
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Menu");
    }
}
