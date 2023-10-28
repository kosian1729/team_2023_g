using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBulletController : MonoBehaviour
{
    [SerializeField] private int hitPower;
    private float X,Y;
    private float timer;

    void Start()
    {
        Destroy(this.gameObject,30.0f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        X = 0.9f * Time.deltaTime;
        Y = Mathf.Sin(timer * Mathf.PI/3) * 0.1f * Time.deltaTime;
        transform.position += transform.rotation * new Vector3(-X,Y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(hitPower);
            Destroy(this.gameObject);
        }
    }
}
