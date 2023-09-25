using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [Header("HeartImageControllerを順番に入れる(0番は空けてください)")]
    [SerializeField] private HeartImageController[] heartImageController;

    private int before_maxHp,before_hp = 11;


    public void SetHeart(int maxHp, int hp)
    {
        //前回からmaxHpの更新があるかチェック
        if(before_maxHp!=maxHp)
        {
            if(maxHp > heartImageController.Length-1)
            {
                Debug.Log("HPの最大値が表示上限を超えています。");
                return;
            }

            //maxHpを超えたハートの画像は表示しないようにする。
            for(int i=heartImageController.Length-1; i>maxHp; i--)
            {
                heartImageController[i].ChangeHeartState(0);
            }
        }

        //回復時の処理
        if(hp>before_hp)
        {
            for(int j=before_hp; j<hp; j++)
            {
                heartImageController[j+1].ChangeHeartState(1);
            }
        }
        else if(hp<before_hp)
        {
            //現在のhpに応じて、一部のハートをダメージ受けた状態にする。
            for(int j=maxHp; j>hp; j--)
            {
                heartImageController[j].ChangeHeartState(2);
            }
        }

        before_maxHp = maxHp;
        before_hp = hp;
    }
}
