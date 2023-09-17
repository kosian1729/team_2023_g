using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletController : MonoBehaviour
{
    [Header("弾速")]
    [SerializeField] private float bulletSpeed;

    [Header("威力")]
    [SerializeField] private int power;

    [Header("弾が消えるまでの時間")]
    [SerializeField] private float duration;

    [Header("衝突時パーティクル")]
    [SerializeField] private GameObject particleObj;

    private float timer;　//弾の生存時間

    void Update()
    {
        //弾の移動
        this.gameObject.transform.position += new Vector3(Time.deltaTime * bulletSpeed,0);

        //弾が時間経過で消える処理
        timer += Time.deltaTime;
        if(timer>=duration)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<IDamagable>().AddDamage(power);
            Instantiate(particleObj,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(other.tag == "Obstacle")
        {
            Instantiate(particleObj,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }

    }
}
