using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMachineController : MonoBehaviour, IDamagable
{
    [Header("敵のスピード")]
    [Tooltip("標準値は1です。")]
    [SerializeField] private float speed;

    [Header("敵の体力")]
    [SerializeField] private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    [Header("進行方向")]
    [Range(-90,90)]
    [SerializeField] private float angle;

    [Header("回転運動をするか")]
    [SerializeField] private MovePatern movePatern;

    [Header("回転の大きさ(回転する場合)")]
    [SerializeField] private float coasterScale = 1;

    [Header("回転するまでの時間")]
    [SerializeField] private float timeOffset = 2.5f;


    private float timer;    //スポーンしてからの経過時間

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

        switch(movePatern)
        {
            case MovePatern.Straight:
                transform.position += Quaternion.Euler(0,0,-angle) * -Vector3.right * Time.deltaTime * speed;
                break;
            
            case MovePatern.Coaster:
                if(timer < timeOffset)
                {
                    transform.position += Quaternion.Euler(0,0,-angle) * -Vector3.right * Time.deltaTime *speed;
                }
                else if(timer >= timeOffset && timer <= timeOffset + 2 * coasterScale )
                {
                    var phase = (timer - timeOffset)/coasterScale * Mathf.PI;   // 2 * coasterScale 秒で一回転
                    transform.position += Quaternion.Euler(0,0,-angle) * new Vector3((-Mathf.Cos(phase)),Mathf.Sin(phase)) * Time.deltaTime * speed;
                }
                else if(timer> timeOffset + 2 * coasterScale)
                {
                    transform.position += Quaternion.Euler(0,0,-angle) * -Vector3.right * Time.deltaTime *speed;
                }
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

    public enum MovePatern
    {
        Straight,
        Coaster
    }
}



