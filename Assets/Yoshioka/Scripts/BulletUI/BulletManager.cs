using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private int slotNum;　//現在のスロット番号
    public BulletInfo[] bulletSlot;
    public RectTransform frame;
    private Vector2 frameOriginalPos;
    public TextMeshProUGUI[] bulletNumText;

    void Start()
    {
        frameOriginalPos = frame.position;

        //Debug
        SetBulletSlot(bulletSlot[0],bulletSlot[1],bulletSlot[2]);
    }

    //Stage開始時に設定される
    public void SetBulletSlot(BulletInfo bullet_0,BulletInfo bullet_1,BulletInfo bullet_2)
    {
        bulletSlot[0] = bullet_0;　//基本的には通常弾が入る
        bulletSlot[1] = bullet_1;
        bulletSlot[2] = bullet_2;

        bulletNumText[0].text = "×"+ bulletSlot[0].runTimeNum;
        bulletNumText[1].text = "×"+ bulletSlot[1].runTimeNum;
        bulletNumText[2].text = "×"+ bulletSlot[2].runTimeNum;

        slotNum = 0;
    }

    public void ChangeSlotNum(int d)
    {
        slotNum += (int)Mathf.Sign(d);　//Mathf.Signは-1か1を返す(なぜかfloat型で返ってくるのでint型にキャストが必要)
        if(slotNum>2)
        {
            slotNum = 0;
        }
        else if(slotNum<0)
        {
            slotNum = 2;
        }

        frame.position = new Vector2(frameOriginalPos.x + 140*slotNum, frameOriginalPos.y);
    }

    public string GetBulletName()
    {
        return bulletSlot[slotNum].name;
    }

    public GameObject GetBulletObj()
    {
        return bulletSlot[slotNum].obj;
    }

    public Sprite GetBulletSprite()
    {
        return bulletSlot[slotNum].sprite;
    }

    public float GetBulletInterval()
    {
        return bulletSlot[slotNum].interval;
    }

    public float GetBulletNum()
    {
        return bulletSlot[slotNum].runTimeNum;
    }

    public void ChangeBulletNum(int a)
    {
        bulletSlot[slotNum].runTimeNum += a;    //残弾数の加算（減算）
        bulletNumText[slotNum].text = "×"+ bulletSlot[slotNum].runTimeNum;  //SlotのTextの更新
    }
}

//Bulletの情報を格納するクラス
[CreateAssetMenu]
public class BulletInfo : ScriptableObject, ISerializationCallbackReceiver
{
    public string name;
    public GameObject obj;
    public Sprite sprite;
    public float interval;

    [Header("ステージ開始時の装弾数")]
    public int initialNum;
    [NonSerialized]
    public int runTimeNum;  //Play中はこちらの値を動かす

    public void OnAfterDeserialize()
    {
        runTimeNum = initialNum;
    }

    public void OnBeforeSerialize() { }
}
