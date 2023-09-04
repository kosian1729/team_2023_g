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
    private float radian;

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
        radian = (angle/180) * Mathf.PI;
        transform.position += new Vector3(-Mathf.Cos(radian),Mathf.Sin(radian)) * Time.deltaTime *speed;
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

