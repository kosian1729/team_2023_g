using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBulletController : MonoBehaviour
{
    [Header("弾速")]
    [SerializeField] private float bulletSpeed;

    [Header("威力")]
    [SerializeField] private int power;

    [Header("弾が消えるまでの時間")]
    [SerializeField] private float duration;

    private float timer;　//弾の生存時間

    void Update()
    {
        //弾の移動
        transform.position += transform.rotation * new Vector3(0,-1) * Time.deltaTime * bulletSpeed;

        //弾が時間経過で消える処理
        timer += Time.deltaTime;
        if(timer>=duration)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(power);
            Destroy(this.gameObject);
        }

    }
}
