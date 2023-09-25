using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSeaDragonController : MonoBehaviour, IDamagable
{
    [Header("敵のスピード")]
    [Tooltip("標準値は1です。")]
    [SerializeField] private float speed;

    [Header("敵の体力")]
    [SerializeField] private int maxHp;

    private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    [Header("敵の攻撃弾")]
    [SerializeField] private GameObject bullet;

    [Header("攻撃間隔")]
    [SerializeField] private float interval;

    [Header("プレイヤーをねらって打つか")]
    [SerializeField] private bool isAim;

    [Header("HPバー")]
    public Slider hpBar;

    [Header("Bossを撃破後のイベント")]
    public GameEvent AfterBoss;

    private Transform player;

    private PolygonCollider2D collider;

    private int phase;

    private float timer;    //スポーンしてからの経過時間
    private int num; //攻撃するたびに加算

    void Start()
    {
        phase = 0;
        num = 1;

        hp = maxHp;
        hpBar.value = hp;

        if(hp<=0.0f)
        {
            Debug.Log(this.gameObject.name + "の体力が設定されていないか、0未満です。");
            Destroy(this.gameObject);
        }

        player = GameObject.FindWithTag("Player").transform;

        //当たり判定を無効化
        collider = GetComponent<PolygonCollider2D>();
        collider.enabled = false;
    }


    void Update()
    {
        if(phase == 0) return;

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
        if(isAim)
        {
            Vector3 direction = player.position - this.transform.position;
            for(int n = 4; n>=-4; n--)
            {
                Instantiate(bullet,this.transform.position,Quaternion.FromToRotation(Vector3.up,Quaternion.Euler(0,0,5*n) * direction));
            }

        }
        else
        {
            Instantiate(bullet,this.transform.position,Quaternion.Euler(0,0,Random.Range(85.0f,95.0f)));
        }

    }

    public void BossStart()
    {
        collider.enabled = true;
        phase = 1;
    }


    //プレイヤーの弾に当たったとき呼び出される
    public void AddDamage(int damage, bool obstacle = false)
    {
        hp-=damage;
        hp = Mathf.Clamp(hp,0,hp);
        hpBar.value = hp;
        if(hp<=0)
        {
            AfterBoss.Raise();
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

