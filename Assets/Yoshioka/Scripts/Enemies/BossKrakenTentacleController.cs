using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKrakenTentacleController : MonoBehaviour ,IDamagable
{
    [SerializeField] private int maxHp;
    private int hp;
    [SerializeField] private int hitPower = 1;
    [SerializeField] GameEvent OnDamageTentacle;

    void Start()
    {
        hp = maxHp;
        Destroy(this.gameObject,6.0f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(hitPower);
        }
    }

    public void AddDamage(int damage, bool obstacle = false)
    {
        hp -= damage;
        OnDamageTentacle.Raise();

        Debug.Log("TentacleHP "+ hp);
        if(hp<=0)
        {   
            Destroy(this.gameObject);
        }
    }
}
