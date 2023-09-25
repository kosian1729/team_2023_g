using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusController : MonoBehaviour, IDamagable
{
    [Header("敵のスピード")]
    [Tooltip("標準値は1です。")]
    [SerializeField] private float speed;

    [Header("敵の体力")]
    [SerializeField] private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    [Header("動き方のパラメーター")]
    [SerializeField] private float x;
    [SerializeField] private float y;

    [Header("ドロップアイテム")]
    [SerializeField] private GameObject dropItem;

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

        var pos = transform.position;

        pos.x -= Mathf.Abs(Mathf.Cos(timer*Mathf.PI/x)) * Time.deltaTime * speed;
        pos.y += Mathf.Sin(timer*Mathf.PI/y) * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。

        transform.position = pos;
    }

    //プレイヤーの弾に当たったとき呼び出される
    public void AddDamage(int damage, bool obstacle = false)
    {
        hp-=damage;
        if(hp<=0)
        {
            if(dropItem)
            {
                Instantiate(dropItem,transform.position,Quaternion.identity);
            } 
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
