using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private CircleCollider2D collider;

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        collider.radius = Mathf.Lerp(collider.radius,1.1f,Time.deltaTime * 4.0f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(1);
        }
    }
}
