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

    private Transform player;

    private int phase;

    private float timer;    //スポーンしてからの経過時間
    private int num; //攻撃するたびに加算
    private float _speed;

    void Start()
    {
        phase = 0;
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

        switch(phase)
        {
            case 0:
                if(timer>1.0f)
                {
                    phase = 1;
                    timer = 0;
                }
                break;

            case 1:
                Vector3 direction = (player.position - this.transform.position);
                this.transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.FromToRotation(Vector3.up,Quaternion.Euler(0,0,-86)*direction),0.4f);

                if(timer>2.0f)
                {
                    _speed = speed;

                    phase = 2;

                    if(num>=2)
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
                    num++;
                }

                timer = 0;
                break;

            case 3:
                transform.position += transform.rotation * new Vector3(-1,0) * Time.deltaTime * _speed;
                _speed -= speed * Time.deltaTime/5.0f;
                _speed = Mathf.Clamp(_speed,2.0f,speed);

                Destroy(this.gameObject,8.0f);
                break;
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

