using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header("動きのパターン（テスト）")]
    public PatternList pattern;
    const float LEN = 2f;

    [Header("敵のスピード")]
    [Tooltip("標準値は1です。")]
    [SerializeField] private float speed;

    [Header("敵の体力")]
    [SerializeField] private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    private float timer;

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
        timer += Time.deltaTime;

        var pos = transform.position;

        switch(pattern)
        {
            //振幅小さめのサインカーブ
            case PatternList.SinCurve_1:
                pos.x -= speed * Time.deltaTime;
                pos.y += Mathf.Cos(timer) * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。
                break;
            
            //振幅大きめのサインカーブ
            case PatternList.SinCurve_2:
                pos.x -= speed * Time.deltaTime;
                pos.y += Mathf.Cos(timer) * Time.deltaTime * 2;
                break;

            //振幅小さめの上下運動
            case PatternList.UpDown_1:
                pos.y -= Mathf.Cos(timer * speed) * Time.deltaTime;
                break;
            
            //振幅大きめの上下運動
            case PatternList.UpDown_2:
                pos.y -= Mathf.Cos(timer * speed) * Time.deltaTime * 2;
                break;

            //直進
            case PatternList.Straight:
                pos.x -= speed * Time.deltaTime;
                break;
        }

        transform.position = pos;
    }

    //プレイヤーの弾に当たったとき呼び出される
    public void AddDamage(int damage)
    {
        hp-=damage;
        if(hp<=0)
        {
            //死亡時の演出などをつけるならここ
            Destroy(this.gameObject);
        }
    }

    //プレイヤーとの衝突時の処理
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(hitPower);
        }
    }
    
}

public enum PatternList
{
    SinCurve_1,
    SinCurve_2,
    UpDown_1,
    UpDown_2,
    Straight,
}
