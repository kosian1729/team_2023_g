using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private GameObject dragonBullet;

    [Header("敵の攻撃弾")]
    [SerializeField] private GameObject dragonBreathBullet;

    [Header("敵の攻撃弾")]
    [SerializeField] private GameObject dragonRocketBullet;

    [Header("HPバー")]
    public Slider hpBar;

    [Header("Bossを撃破後のイベント")]
    public GameEvent AfterBoss;

    private Transform player;

    private PolygonCollider2D collider;

    private int phase;

    private float moveTimer;    
    private float timer;
    private int num;

    void Start()
    {
        phase = 0;
        num = 0;

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
                moveTimer += Time.deltaTime;
                pos.y += Mathf.Cos(moveTimer/2) * 1.5f * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。

                if(timer>4.5f - 0.15f * (maxHp - hp)/10)
                {
                    phase = Random.Range(1,102)%3 + 2;
                    timer = 0;
                    num = (maxHp - hp)/22 + 1;
                }
                break;

            case 2:
                moveTimer += Time.deltaTime;
                pos.y += Mathf.Cos(moveTimer/2) * 1.5f * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。

                if(num>0 && timer > 0.4f)
                {
                    Attack();
                    timer = 0;
                    num--;
                }
                else if(num<=0 && timer > 0.4f)
                {
                    phase = 1;
                }
                break;

            case 3:
                StartCoroutine(Breath());
                phase = 1;
                break;
            
            case 4:
                StartCoroutine(Rocket());
                phase = 1;
                break;
        }

        transform.position = pos;
    }

    void Attack()
    {
        Vector3 direction = player.position - this.transform.position;
        int p = (maxHp - hp)/30;
        for(int n = 2+p; n>=-2-p; n--)
        {
            Instantiate(dragonBullet,this.transform.position,Quaternion.FromToRotation(Vector3.up,Quaternion.Euler(0,0,6*n) * direction));
        }
    }

    IEnumerator Breath()
    {
        Vector3 direction = player.position - this.transform.position;
        for(int n = 3; n>=-3; n--)
        {
            Instantiate(dragonBreathBullet,this.transform.position,Quaternion.FromToRotation(Vector3.up,Quaternion.Euler(0,0,8*n) * direction));
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator Rocket()
    {
        
        int q = (maxHp - hp)/33;
        for(int n = 3+q; n > 0; n--)
        {
            Vector3 direction = player.position - this.transform.position;
            Instantiate(dragonRocketBullet,this.transform.position,Quaternion.FromToRotation(Vector3.up,direction));
            yield return new WaitForSeconds(0.6f);
        }
    }

    public void BossStart()
    {
        collider.enabled = true;
        phase = 1;
    }


    //プレイヤーの弾に当たったとき呼び出される
    public void AddDamage(int damage)
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

