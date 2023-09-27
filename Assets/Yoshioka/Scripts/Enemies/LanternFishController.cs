using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFishController : MonoBehaviour, IDamagable
{
    [Header("敵のスピード")]
    [Tooltip("標準値は1です。")]
    [SerializeField] private float speed;

    [Header("敵の体力")]
    [SerializeField] private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    public Transform lanternOffset;

    public GameObject lanternBullet;

    private float timer;
    private float attackTimer;

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

        var pos = transform.position;
        pos.x -= Mathf.Abs(Mathf.Cos(timer*Mathf.PI/3)) * Time.deltaTime * speed;
        transform.position = pos;

        attackTimer += Time.deltaTime;

        if(attackTimer > 0.6f)
        {
            attackTimer = 0.0f;
            Instantiate(lanternBullet,lanternOffset.position,Quaternion.Euler(0,0,Random.Range(-40,-14)));
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
