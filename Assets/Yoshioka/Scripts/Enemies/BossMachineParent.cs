using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMachineParent : MonoBehaviour, IDamagable
{
    public BossMachineController boss;

    public int hitPower;

    public void AddDamage(int damage, bool obstacle = false)
    {
        boss.AddDamage(damage);
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
