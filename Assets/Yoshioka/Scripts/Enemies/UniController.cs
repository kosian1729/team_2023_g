using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UniController : MonoBehaviour, IDamagable
{
    [Header("敵の動く幅")]
    [Tooltip("標準値は1です。")]
    [SerializeField] private float moveHeight = 1;

    [Header("敵の体力")]
    [SerializeField] private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    [Header("ウニの針（弾）")]
    [SerializeField] private GameObject bullet;

    [Header("発射数(0の時は上下に動くのみになります。)")]
    [SerializeField] private int attackNum;

    [Header("攻撃間隔")]
    [SerializeField] private float interval = 4.0f;

    [Header("目（開）の画像")]
    public Sprite openEyeSprite;

    [Header("目（閉）の画像")]
    public Sprite closeEyeSprite;

    [Header("空の画像")]
    public Sprite noneSprite;

    private Transform player;

    private SpriteRenderer spriteRenderer;

    private int phase;

    private float timer;    //スポーンしてからの経過時間
    private float lastTime;   //Cosの位相記録用

    void Start()
    {
        phase = 0;
        lastTime = 0;

        if(attackNum == 0)
        {
            phase = -1;
        }

        if(hp<=0.0f)
        {
            Debug.Log(this.gameObject.name + "の体力が設定されていないか、0未満です。");
            Destroy(this.gameObject);
        }

        player = GameObject.FindWithTag("Player").transform;
        spriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = noneSprite;
    }


    void Update()
    {
        timer += Time.deltaTime;
        var pos = transform.position;

        switch(phase)
        {
            case 0:
                if(timer>1.0f)
                {
                    phase = 1;
                    timer = 0;
                    spriteRenderer.sprite = noneSprite;
                }
                break;

            case 1:
                pos += transform.rotation * Vector3.up * Mathf.Cos(timer/moveHeight + lastTime) * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。

                if(timer > interval)
                {
                    lastTime += timer/moveHeight;
                    phase = 2;
                    timer = 0;
                    spriteRenderer.sprite = closeEyeSprite;
                }
                break;

            case 2:
                if(timer > 1.0f)
                {
                    phase = 3;
                    timer = 0;
                    AttackPhase();
                }
                break;

            case 3:

                break;

            case -1:
                pos += transform.rotation * Vector3.up * Mathf.Cos(timer/moveHeight) * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。
                break;
        }

        transform.position = pos;
    }

    public void AttackPhase()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 1.0f)) //0-1.0
                .Append(this.transform.DOScale(new Vector3(1.1f,1.1f,1.1f),0.4f))   //1.0-1.4
                .Append(this.transform.DOScale(new Vector3(1.0f,1.0f,1.0f),0.4f))   //1.4-1.8
                .AppendCallback(() => {
                    phase = 0;
                });

        sequence.InsertCallback(1.0f,() =>{
            spriteRenderer.sprite = openEyeSprite;
        });

        sequence.InsertCallback(1.2f,() =>{
            Attack();
        });
    }

    public void Attack()
    {
        float attackAngle;
        float offset = Random.Range(0.0f,360/attackNum);

        for(int n = 0; n<attackNum; n++)
        {
            attackAngle = (360/attackNum) * n + offset;
            //AudioManager.Instance.PlaySE("SEウニ発射");
            Instantiate(bullet,this.transform.position,Quaternion.Euler(0,0,attackAngle));
        }
    }

    //プレイヤーの弾に当たったとき呼び出される
    public void AddDamage(int damage,bool obstacle = false)
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

