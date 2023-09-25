using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeahouseController : MonoBehaviour, IDamagable
{
    [Header("敵のスピード")]
    [Tooltip("標準値は1です。")]
    [SerializeField] private float speed;

    [Header("敵の体力")]
    [SerializeField] private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    [Header("敵の攻撃弾")]
    [SerializeField] private GameObject bullet;

    [Header("攻撃間隔")]
    [SerializeField] private float interval;

    [Header("プレイヤーをねらって打つか")]
    [SerializeField] private bool isAim;

    private Transform player;

    private int phase;

    private float timer;    //スポーンしてからの経過時間
    private int num; //攻撃するたびに加算

    void Start()
    {
        phase = 1;
        num = 1;

        if(hp<=0.0f)
        {
            Debug.Log(this.gameObject.name + "の体力が設定されていないか、0未満です。");
            Destroy(this.gameObject);
        }

        player = GameObject.FindWithTag("Player").transform;
        }


    void Update()
    {
        timer += Time.deltaTime;
        var pos = transform.position;

        switch(phase)
        {
            case 1:
                pos.x -= Time.deltaTime;
                pos.y += Mathf.Cos(timer) * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。

                if(timer>1.8f)
                {
                    phase = 2;
                }
                break;

            case 2:
                pos.x +=Time.deltaTime * 0.4f;
                pos.y += Mathf.Cos(timer) * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。

                if(timer>1.8f + interval * num)
                {
                    Attack();
                    num++;
                }
                break;
        }

        transform.position = pos;
    }

    void Attack()
    {
        //playerが自分より右に行くと攻撃をやめる
        if(this.transform.position.x < player.position.x) return;

        if(isAim)
        {
            Vector3 direction = player.position - this.transform.position;
            Instantiate(bullet,this.transform.position ,Quaternion.FromToRotation(Vector3.up,direction));
        }
        else
        {
            Instantiate(bullet,this.transform.position,Quaternion.Euler(0,0,Random.Range(85.0f,95.0f)));
        }

    }



    //プレイヤーの弾に当たったとき呼び出される
    public void AddDamage(int damage, bool obstacle = false)
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

