using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidController : MonoBehaviour, IDamagable
{
    [Header("敵のスピード")]
    [Tooltip("標準値は6です。")]
    [SerializeField] private float speed;

    [Header("敵の体力")]
    [SerializeField] private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    [Header("攻撃回数")]
    [SerializeField] private int num = 1;

    [Header("攻撃間隔")]
    [SerializeField] private float interval = 0.9f;

    private Transform player;

    private int phase;

    private float timer;    //スポーンしてからの経過時間
    private float _speed;

    void Start()
    {
        phase = 0;

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

        switch(phase)
        {
            case 0:
                if(timer>interval)
                {
                    phase = 1;
                    timer = 0;
                }
                break;

            case 1:
                Vector3 direction = (player.position - this.transform.position);
                this.transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.FromToRotation(Vector3.up,Quaternion.Euler(0,0,-86)*direction),0.9f);

                if(timer>1.5f)
                {
                    _speed = speed;

                    phase = 2;
                    num--;

                    if(num<=0)
                    {
                        phase = 3;
                    }
                }


                break;

            case 2:
                transform.position += transform.rotation * new Vector3(-1,0) * Time.deltaTime * _speed;
                _speed -= speed * Time.deltaTime/2.0f;

                if(_speed <= 0)
                {
                    phase = 1;
                }

                timer = 0;
                break;

            case 3:
                transform.position += transform.rotation * new Vector3(-1,0) * Time.deltaTime * _speed;
                _speed -= speed * Time.deltaTime/5.0f;
                _speed = Mathf.Clamp(_speed,speed*0.75f,speed);

                Destroy(this.gameObject,8.0f);
                break;
        }
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

