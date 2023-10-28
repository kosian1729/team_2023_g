using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossUniController : MonoBehaviour, IDamagable
{
    [Header("敵の体力")]
    [SerializeField] private int maxHp;

    private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    [Header("ウニの針（弾）")]
    [SerializeField] private GameObject normalBullet;
    [SerializeField] private GameObject rageBullet;

    [Header("攻撃間隔")]
    [SerializeField] private float interval = 4.0f;

    [Header("目（開）の画像")]
    public Sprite openEyeSprite;

    [Header("目（閉）の画像")]
    public Sprite closeEyeSprite;

    [Header("空の画像")]
    public Sprite noneSprite;

    [Header("HPバー")]
    public Slider hpBar;

    [Header("Bossを撃破後のイベント")]
    public GameEvent AfterBoss;

    private Transform player;
    private SpriteRenderer eyeSpriteRenderer;
    private SpriteRenderer uniSpriteRenderer;

    private CircleCollider2D collider;

    private int phase;

    private float timer;    //スポーンしてからの経過時間
    private int num; //攻撃するたびに加算

    public Vector2 direction;
    public float oriSpeed;
    private float speed;
    public float offset;

    void Start()
    {
        phase = 0;
        num = 1;

        speed = oriSpeed;
        hp = maxHp;
        hpBar.value = hp;

        if(hp<=0.0f)
        {
            Debug.Log(this.gameObject.name + "の体力が設定されていないか、0未満です。");
            Destroy(this.gameObject);
        }

        player = GameObject.FindWithTag("Player").transform;

        eyeSpriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        eyeSpriteRenderer.sprite = closeEyeSprite;
        uniSpriteRenderer = GetComponent<SpriteRenderer>();
        uniSpriteRenderer.color = Color.white;

        //当たり判定を無効化
        collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
    }

    public void BossStart()
    {
        collider.enabled = true;
        timer = 0;
        phase = 1;
    }


    void Update()
    {
        if(phase == 0) return;

        timer += Time.deltaTime;

        switch(phase)
        {
            case 1:
                BoundWall(Camera.main);
                //位置更新
                transform.position += GetVelocity(direction, speed);
                //オブジェクトの向き更新
                transform.rotation = LookDirection(direction);

                if(timer > interval)
                {
                    AttackPhase();
                    timer = 0;
                }

                //HP50%以下で怒り状態になる
                if(hp <= maxHp/2)
                {
                    RagePhase();
                    phase = 0;
                }
                break;

            case 2:
                BoundWall(Camera.main);
                //位置更新
                transform.position += GetVelocity(direction, speed);
                //オブジェクトの向き更新
                transform.rotation = LookDirection(direction);

                if(timer > interval * 0.75f)
                {
                    RageAttackPhase();
                    timer = 0;
                }
                break;
        }
    }

    public void AttackPhase()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOScale(new Vector3(1.7f,1.7f,1.7f), 1.0f)) //0-1.0
                .Append(this.transform.DOScale(new Vector3(2.1f,2.1f,2.1f),0.4f))   //1.0-1.4
                .Append(this.transform.DOScale(new Vector3(2.0f,2.0f,2.0f),0.4f)); //1.4-1.8

        sequence.InsertCallback(1.0f,() =>{
            eyeSpriteRenderer.sprite = openEyeSprite;
        });

        sequence.InsertCallback(1.2f,() =>{
            Attack(Random.Range(7,18),normalBullet);
            eyeSpriteRenderer.sprite = closeEyeSprite;
        });
    }

    public void Attack(int attackNum,GameObject bullet)
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

    //HP50%以下で怒り状態になる
    public void RagePhase()
    {
        //減速する
        DOVirtual.Float(oriSpeed,0,1,value =>{
            speed = value;
        });

        //色が赤くなる
        uniSpriteRenderer.DOColor(Color.red,2.0f).OnComplete(() =>{
            ToRageAttackPhase();
        });
    }

    public void ToRageAttackPhase()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOScale(new Vector3(1.7f,1.7f,1.7f), 1.0f)) //0-1.0
                .Append(this.transform.DOScale(new Vector3(2.1f,2.1f,2.1f),0.4f))   //1.0-1.4
                .Append(this.transform.DOScale(new Vector3(1.8f,1.8f,1.8f),0.6f))   //1.4-2.0
                .Append(this.transform.DOScale(new Vector3(2.1f,2.1f,2.1f),0.4f))   //2.0-2.4
                .Append(this.transform.DOScale(new Vector3(1.8f,1.8f,1.8f),0.6f))   //2.4-3.0
                .Append(this.transform.DOScale(new Vector3(2.1f,2.1f,2.1f),0.4f))   //3.0-3.4
                .Append(this.transform.DOScale(new Vector3(2.0f,2.0f,2.0f),0.4f))   //3.4-3.8
                .AppendCallback(() => {
                    speed = oriSpeed * 1.2f;
                    timer = 0;
                    phase = 2;
                });

        sequence.InsertCallback(1.0f,() =>{
            eyeSpriteRenderer.sprite = openEyeSprite;
        });

        sequence.InsertCallback(1.3f,() =>{
            //AudioManager.Instance.PlaySE("SEイーゲル覚醒発射1");
            Attack(14,rageBullet);
            eyeSpriteRenderer.sprite = closeEyeSprite;
        });

        sequence.InsertCallback(2.0f,() =>{
            eyeSpriteRenderer.sprite = openEyeSprite;
        });

        sequence.InsertCallback(2.3f,() =>{
            //AudioManager.Instance.PlaySE("SEイーゲル覚醒発射2");
            Attack(21,rageBullet);
            eyeSpriteRenderer.sprite = closeEyeSprite;
        });

        sequence.InsertCallback(3.0f,() =>{
            eyeSpriteRenderer.sprite = openEyeSprite;;
        });

        sequence.InsertCallback(3.3f,() =>{
            //AudioManager.Instance.PlaySE("SEイーゲル覚醒発射3");
            Attack(28,rageBullet);
            eyeSpriteRenderer.sprite = closeEyeSprite;
        });
    }

    public void RageAttackPhase()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOScale(new Vector3(1.7f,1.7f,1.7f), 1.0f)) //0-1.0
                .Append(this.transform.DOScale(new Vector3(2.1f,2.1f,2.1f),0.4f))   //1.0-1.4
                .Append(this.transform.DOScale(new Vector3(1.8f,1.8f,1.8f),0.6f))   //1.4-2.0
                .Append(this.transform.DOScale(new Vector3(2.1f,2.1f,2.1f),0.4f))   //2.0-2.4
                .Append(this.transform.DOScale(new Vector3(2.0f,2.0f,2.0f),0.4f));   //2.4-2.8

        sequence.InsertCallback(1.0f,() =>{
            eyeSpriteRenderer.sprite = openEyeSprite;
        });

        sequence.InsertCallback(1.3f,() =>{
            Attack(Random.Range(14,22),rageBullet);
            eyeSpriteRenderer.sprite = closeEyeSprite;
        });

        sequence.InsertCallback(2.0f,() =>{
            eyeSpriteRenderer.sprite = openEyeSprite;
        });

        sequence.InsertCallback(2.3f,() =>{
            Attack(Random.Range(22,29),rageBullet);
            eyeSpriteRenderer.sprite = closeEyeSprite;
        });
    }


    //壁に当たっているかどうか
    private bool is_hit_wall = false;

    void BoundWall(Camera camera)
    {
        is_hit_wall = false;

        Vector3 change_position = transform.position;
        float top = Camera.main.transform.position.y + GetHalfHeight(camera);
        float bottom = Camera.main.transform.position.y - GetHalfHeight(camera);
        float right = Camera.main.transform.position.x + GetHalfWidth(camera);
        float left = Camera.main.transform.position.x - GetHalfWidth(camera);

        //左の壁判定
        if (transform.position.x <= left  +offset)
        {
            direction.x *= -1.0f;
            change_position.x = left  +offset+ float.Epsilon;
            is_hit_wall = true;
        }
        //右の壁判定
        else if (transform.position.x >= right -offset)
        {
            direction.x *= -1.0f;
            change_position.x = right -offset -float.Epsilon;
            is_hit_wall = true;
        }
        //上の壁判定
        if (transform.position.y >= top -offset)
        {
            direction.y *= -1.0f;
            change_position.y = top - offset -float.Epsilon;
            is_hit_wall = true;
        }
        //下の壁判定
        else if (transform.position.y <= bottom +offset)
        {
            direction.y *= -1.0f;
            change_position.y = bottom +offset + float.Epsilon;
            is_hit_wall = true;
        }

        transform.position = change_position;
    }

    //向きと速度から速度ベクトルを計算する
    Vector3 GetVelocity(Vector2 direction, float speed)
    {
        return direction * speed * Time.deltaTime;
    }

    //指定した方向を返す
    Quaternion LookDirection(Vector2 direction)
    {
        return Quaternion.FromToRotation(Vector3.up, direction);
    }

    //カメラの横のサイズの半分を取得
    float GetHalfWidth(Camera camera)
    {
        return camera.orthographicSize * camera.aspect;
    }
    //カメラの縦のサイズの半分を取得
    float GetHalfHeight(Camera camera)
    {
        return camera.orthographicSize;
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

