using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour, IDamagable
{
    private Camera cam;
    private GameObject animPosObj;

    [Header("ロストテクノロジー「ワープ」")]
    [SerializeField] private bool canWarp;
    [Header("Playerの移動速度")]
    [SerializeField] private float playerSpeed;
    [Header("Playerの体力")]
    [SerializeField] private int maxHp;
    private int hp;　//Playerの現在HP

    private float timer;　//弾の発射間隔用のタイマー

    [Header("発射位置")]
    [SerializeField] private float offset_x;
    [Header("UIのハートを制御するスクリプト")]
    public HeartManager heartManager;
    [Header("Bulletを管理するスクリプト")]
    public BulletManager bulletManager;
    [Header("GameOverイベント")]
    public GameEvent GameOver;
    [Header("Bossイベント")]
    public GameEvent Boss;

    public Animator animator;
    private SpriteRenderer spriteRnderer;

    private Image damagePanel;

    private bool stop;

    private bool noDamageMode;

    [Header("無敵時間")]
    public float invincibilityDuration = 1.0f; // 無敵時間（秒）

    private float invincibilityTimer = 0.0f;   // 経過時間を格納するタイマー変数(初期値0秒)
    private bool isInvincible = false;         // 無敵状態かどうかのフラグ

    void Start()
    {
        cam = Camera.main;
        animPosObj = transform.parent.gameObject;
        spriteRnderer = GetComponent<SpriteRenderer>();

        damagePanel = GameObject.Find("DamagePanel").GetComponent<Image>();
        damagePanel.color = Color.clear;

        hp = maxHp;
        heartManager.SetHeart(maxHp, hp);
        //animator = transform.parent.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (stop) return;

        Move();
        Attack();
        Select();

        //無敵状態フラグがTrueのときに毎フレーム実行
        if (isInvincible)
        {
            //ここに無敵状態のときの処理を書く
            Debug.Log("無敵状態");

            //毎フレームタイマー変数にTime.deltaTimeを足す
            invincibilityTimer += Time.deltaTime;

            //タイマーが無敵時間を超えたとき
            if (invincibilityTimer >= invincibilityDuration)
            {
                Debug.Log("無敵状態終わり");

                //無敵状態フラグをFalseにする
                isInvincible = false;
                //タイマーを0.0秒にリセットする
                invincibilityTimer = 0.0f;
            }
        }
    }

    //Playerを、入力に応じた方向へと移動させる。
    void Move()
    {
        //WASDが入力されると、-1~1の整数値が返される。（方向指定用）
        float x = Input.GetAxisRaw("AD");
        float y = Input.GetAxisRaw("WS");

        //カメラの端を超えている時、プレイヤーがはみ出ないようにする。
        var currentPos = transform.localPosition + new Vector3(x * Time.deltaTime * playerSpeed, y * Time.deltaTime * playerSpeed);
        var gap = animPosObj.transform.localPosition;

        currentPos.x = Mathf.Clamp(currentPos.x, -cam.orthographicSize * 1920/1080 -gap.x, cam.orthographicSize * 1920/1080 -gap.x);


        if(!canWarp)
        {
            currentPos.y = Mathf.Clamp(currentPos.y, -cam.orthographicSize-gap.y, cam.orthographicSize-gap.y);
        }
        else
        {
            if(transform.position.y > cam.orthographicSize + 0.2f)
            {
                currentPos.y = -cam.orthographicSize - 0.2f - gap.y;
            }
            else if(transform.position.y < -cam.orthographicSize - 0.2f)
            {
                currentPos.y = cam.orthographicSize - 0.2f -gap.y;
            }
        }
        

        transform.localPosition = currentPos;

    }

    void Attack()
    {
        // スペースキーを押している間、一定間隔でbulletを打ち続ける
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) && (timer <= 0.0f) && (bulletManager.GetBulletNum() > 0))
        {
            AudioManager.Instance.PlaySE("SE攻撃");

            Instantiate(bulletManager.GetBulletObj(), new Vector3(transform.position.x + offset_x,transform.position.y), Quaternion.identity);
            bulletManager.ChangeBulletNum(-1,bulletManager.GetSlotNum());

            timer = bulletManager.GetBulletInterval(); // 間隔をセット
        }
        // タイマーの値を減らす
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bulletManager.ChangeSlotNum(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bulletManager.ChangeSlotNum(1);
        }
    }

    //被ダメ時(回復時）
    public void AddDamage(int damage, bool obstacle = false)
    {
        if (!obstacle)
        {
            if (noDamageMode) return;
            if (isInvincible) return;
        }
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHp);

        heartManager.SetHeart(maxHp, hp);

        if(damage>0)
        {
            damagePanel.color = new Color(0.8f,0f,0f,0.8f);
            damagePanel.DOFade(0,0.3f).SetEase(Ease.InQuad);
        }

        if (hp <= 0)
        {
            //GameOver処理
            GameOver.Raise();

            this.gameObject.SetActive(false);
        }
        else
        {
            //敵の弾に当たった時に無敵状態フラグをTrueにする
            isInvincible = true;
            //点滅演出開始
            StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        while(isInvincible)
        {
            spriteRnderer.enabled = false;

            yield return new WaitForSeconds(0.1f);

            spriteRnderer.enabled = true;

            yield return new WaitForSeconds(0.1f);
        }

        spriteRnderer.enabled = true;
    }



    public void StopControll(bool isStop)
    {
        stop = isStop;
        noDamageMode = isStop;

    }

    public void BeforeStageStart()
    {
        animator.SetTrigger("PlayerIN");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BossArea")
        {
            Boss.Raise();
        }
    }
}
