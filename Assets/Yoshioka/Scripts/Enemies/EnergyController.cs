using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour, IDamagable
{
    public int maxHp;
    private int hp;
    public GameObject energyBullet;
    public GameObject bomEffect;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void EnergyAttack()
    {
        Instantiate(energyBullet,transform.position,Quaternion.Euler(0,0,Random.Range(5,46)));
    }

    public IEnumerator TripleEnergyAttack()
    {
        Instantiate(energyBullet,transform.position,Quaternion.Euler(0,0,Random.Range(5,16)));
        yield return new WaitForSeconds(0.2f);
        Instantiate(energyBullet,transform.position,Quaternion.Euler(0,0,Random.Range(25,36)));
        yield return new WaitForSeconds(0.2f);
        Instantiate(energyBullet,transform.position,Quaternion.Euler(0,0,Random.Range(45,56)));
    }

    public void AddDamage(int damage, bool obstacle = false)
    {
        hp -= damage;

        spriteRenderer.color = new Color(0.5f * (1.0f-(float)hp/(float)maxHp),1,0,1);

        if(hp<0)
        {
            Instantiate(bomEffect,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    //プレイヤーとの衝突時の処理
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(2);
        }
    }
}
