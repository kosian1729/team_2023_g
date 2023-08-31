using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Playerの移動速度")]
    [SerializeField] private float playerSpeed;

    [Header("Playerの体力")]
    [SerializeField] private int maxHp;

    private int hp;　//Playerの現在HP

    [Header("発射の間隔")]
    [SerializeField] private float interval;

    private float timer;　//弾の発射間隔用のタイマー

    [Header("発射位置")]
    [SerializeField] private float offset_x;

    [Header("通常弾のプレハブ")]
    [SerializeField] private GameObject normal_bullet;

    private GameObject[] bullets;

    [Header("UIのハートを制御するスクリプト")]
    public HeartManager heartManager;

    [Header("Bulletを管理するスクリプト")]
    public BulletManager bulletManager;

    public Animator animator;

    private bool stop;

    void Start()
    {
        hp = maxHp;
        heartManager.SetHeart(maxHp,hp);
        //animator = transform.parent.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(stop) return;

        Move();
        Attack();
        Select();
    }

    void Move()
    {
        //WASDが入力されると、-1~1の整数値が返される。（方向指定用）
        float x = Input.GetAxisRaw("AD");
        float y = Input.GetAxisRaw("WS");

        //Playerを、入力に応じた方向へと移動させる。
        this.gameObject.transform.position += new Vector3(x*Time.deltaTime*playerSpeed,y*Time.deltaTime*playerSpeed);
    }

    void Attack()
    {
        // スペースキーを押している間、一定間隔でbulletを打ち続ける
        if((Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.DownArrow)) && (timer <= 0.0f) && (bulletManager.GetBulletNum()>0))
        {
            Instantiate(bulletManager.GetBulletObj(), new Vector3(transform.position.x + offset_x,transform.position.y), Quaternion.Euler(0,0,-90));
            bulletManager.ChangeBulletNum(-1);
            timer = bulletManager.GetBulletInterval(); // 間隔をセット
        }
        // タイマーの値を減らす
        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    void Select()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bulletManager.ChangeSlotNum(-1);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            bulletManager.ChangeSlotNum(1);
        }
    }

    //被ダメ時
    public void AddDamage(int damage)
    {
        hp-=damage;

        heartManager.SetHeart(maxHp,hp);

        if(hp<=0)
        {
            //GameOver処理
        }
    }

    public void StopControll(bool _stop)
    {
        stop = _stop;
    }

    public void BeforeStageStart()
    {
        animator.SetTrigger("PlayerIN");
    }
}
