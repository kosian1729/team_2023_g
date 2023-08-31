using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class BulletManager : MonoBehaviour
{
    private int slotNum;　//現在のスロット番号

    [HideInInspector]
    public BulletInfo[] bulletSlot;

    public StageBulletInfo[] sbiList;

    public RectTransform frame;
    private Vector2 frameOriginalPos;
    public TextMeshProUGUI[] bulletNumText;

    void Start()
    {
        frameOriginalPos = frame.position;

        //Debug
        SetBulletSlot(SearchStageBulletInfo(SceneManager.GetActiveScene().name));
    }

    //Stage開始時に設定される
    public void _SetBulletSlot(BulletInfo bullet_0,BulletInfo bullet_1,BulletInfo bullet_2)
    {
        
        bulletSlot[0] = bullet_0;　//基本的には通常弾が入る
        bulletSlot[1] = bullet_1;
        bulletSlot[2] = bullet_2;

        bulletNumText[0].text = "×"+ bulletSlot[0].num;
        bulletNumText[1].text = "×"+ bulletSlot[1].num;
        bulletNumText[2].text = "×"+ bulletSlot[2].num;


        slotNum = 0;
    }

    public void SetBulletSlot(StageBulletInfo sbi)
    {
        //弾の情報を格納
        bulletSlot[0] = sbi.bullet_0;　//基本的には通常弾が入る
        bulletSlot[1] = sbi.bullet_1;
        bulletSlot[2] = sbi.bullet_2;

        //弾ごとの初期装弾数を取得
        bulletSlot[0].num = sbi.num_0;
        bulletSlot[1].num = sbi.num_1;
        bulletSlot[2].num = sbi.num_2;

        //UI用　残弾数表示
        bulletNumText[0].text = "×"+ bulletSlot[0].num;
        bulletNumText[1].text = "×"+ bulletSlot[1].num;
        bulletNumText[2].text = "×"+ bulletSlot[2].num;
    }

    public StageBulletInfo SearchStageBulletInfo(string _stageName)
    {
        foreach(StageBulletInfo sbi in sbiList)
        {
            if(sbi.stageName == _stageName) return sbi;
        }

        return sbiList[0];
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
        return bulletSlot[slotNum].num;
    }

    public void ChangeBulletNum(int a)
    {
        bulletSlot[slotNum].num += a;    //残弾数の加算（減算）
        bulletNumText[slotNum].text = "×"+ bulletSlot[slotNum].num;  //SlotのTextの更新
    }
}
