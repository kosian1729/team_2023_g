using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header("動きのパターン（テスト）")]
    public int pattern;
    const float LEN = 2f;

    [Header("敵の体力")]
    [SerializeField] private float hp;

    void Start()
    {
        if(hp<=0.0f)
        {
            Debug.Log(this.gameObject.name + "の体力が設定されていないか、0未満です。");
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        var pos = transform.position;

        switch(pattern)
        {
            case 1:
                pos.y = Mathf.Sin(Time.time) * LEN;
                break;

            case 2:
                pos.x += Mathf.Cos(Time.time) * LEN * Time.deltaTime;
                break;
            
            case 3:
                pos.x -= 1*Time.deltaTime;
                pos.y = Mathf.Sin(Time.time*1.2f) * LEN;
                break;
        }

        transform.position = pos;
    }

    //プレイヤーの弾に当たったとき呼び出される
    public void AddDamage(float damage)
    {
        hp-=damage;
        if(hp<=0)
        {
            //死亡時の演出などをつけるならここ
            Destroy(this.gameObject);
        }
    }
    
}
