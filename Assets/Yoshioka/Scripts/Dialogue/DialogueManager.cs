using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public CanvasGroup dialogueObj;
    public TextMeshProUGUI logTextZone;
    public TextMeshProUGUI nameTextZone;
    public RectTransform namePanelRect;
    public Vector2 oriNamePanelRect;
    public Image charaImageZone;
    public RectTransform charaPanelRect;
    private Vector2 oriCharaPanelRect;
    public Sprite sprite_void;
    public DialogueBlock[] logBlocks;

    [Header("次の文字を表示するまでの時間")]
    [SerializeField]
    private float delayDuration = 0.05f;

    private Dialogue dialogue;

    private int logBlockNum;
    private int logNum;

    private bool isDialogue;

    private Coroutine typeWriteCoroutine;

    //CallBack用
    public delegate void OnEndLogDelegate();
    public OnEndLogDelegate OnEndLog;

    void Awake()
    {
        oriNamePanelRect = namePanelRect.anchoredPosition;
        oriCharaPanelRect = charaPanelRect.anchoredPosition;
    }

    //会話をはじめる
    public void StartDialogue(int _logBlockNum)
    {
        logBlockNum = _logBlockNum;
        logNum = 0;

        dialogue = logBlocks[logBlockNum].dialogues[logNum];

        logTextZone.text = "";

        //名前欄
        if(dialogue.name != null)
        {
            //  名前表示の左右反転
            if(dialogue.isRight)
            {
                namePanelRect.anchoredPosition = new Vector2(-oriNamePanelRect.x,oriNamePanelRect.y);
            }
            else
            {
                namePanelRect.anchoredPosition = new Vector2(oriNamePanelRect.x,oriNamePanelRect.y);
            }
            nameTextZone.text = dialogue.name;
        }
        else
        {
            nameTextZone.text = "";
        }   

        //立ち絵欄
        if(dialogue.sprite != null)
        {
            //  立ち絵表示の左右反転
            if(dialogue.isRight)
            {
                charaPanelRect.anchoredPosition = new Vector2(-oriCharaPanelRect.x,oriCharaPanelRect.y);
            }
            else
            {
                charaPanelRect.anchoredPosition = new Vector2(oriCharaPanelRect.x,oriCharaPanelRect.y);
            }

            charaImageZone.sprite = dialogue.sprite;
            charaImageZone.SetNativeSize();
        }
        else
        {
            charaImageZone.sprite = sprite_void;
        }

        dialogueObj.DOFade(1.0f,0.4f).OnComplete(() =>
        {
            isDialogue = true;
            typeWriteCoroutine = StartCoroutine(TypeWriteCoroutine());
        });
    }

    public void Update()
    {
        if(!isDialogue) return;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            //文字を生成中にスペースキー(or左クリック)を押されたら、文字をすべて表示する。
            if(typeWriteCoroutine != null)
            {
                StopCoroutine(typeWriteCoroutine);
                typeWriteCoroutine = null;
                logTextZone.maxVisibleCharacters = dialogue.text.Length;
                return;
            }

            logNum++;

            //logNumが最後までいったら会話を終わる
            if(logNum == logBlocks[logBlockNum].dialogues.Length)
            {
                isDialogue = false;
                dialogueObj.DOFade(0.0f,0.4f).OnComplete(() =>
                {
                    OnEndLog();
                });
                
                return;
            }

            //dialogueを更新
            dialogue = logBlocks[logBlockNum].dialogues[logNum];

            typeWriteCoroutine = StartCoroutine(TypeWriteCoroutine());

            //名前欄
            if(dialogue.name != null)
            {
                //  名前表示の左右反転
                if(dialogue.isRight)
                {
                    namePanelRect.anchoredPosition = new Vector2(-oriNamePanelRect.x,oriNamePanelRect.y);
                }
                else
                {
                    namePanelRect.anchoredPosition = new Vector2(oriNamePanelRect.x,oriNamePanelRect.y);
                }

                nameTextZone.text = dialogue.name;
            }
            else
            {
                nameTextZone.text = "";
            }

            //立ち絵欄
            if(dialogue.sprite != null)
            {
                //  立ち絵表示の左右反転
                if(dialogue.isRight)
                {
                    charaPanelRect.anchoredPosition = new Vector2(-oriCharaPanelRect.x,oriCharaPanelRect.y);
                }
                else
                {
                    charaPanelRect.anchoredPosition = new Vector2(oriCharaPanelRect.x,oriCharaPanelRect.y);
                }

                charaImageZone.sprite = dialogue.sprite;
                charaImageZone.SetNativeSize();
            }
            else
            {
                charaImageZone.sprite = sprite_void;
            }
        }
    }

    private IEnumerator TypeWriteCoroutine()
    {
        var delay = new WaitForSeconds(delayDuration);
        var length = dialogue.text.Length;

        logTextZone.text = dialogue.text;

        for(int i=0; i<length; i++)
        {
            logTextZone.maxVisibleCharacters = i;

            //delayDuration秒分待機
            yield return delay;
        }

        logTextZone.maxVisibleCharacters = length;

        typeWriteCoroutine = null;
    }
}
