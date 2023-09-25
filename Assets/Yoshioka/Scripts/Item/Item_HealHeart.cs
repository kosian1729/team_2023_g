using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HealHeart : MonoBehaviour
{
    [SerializeField] private int healNum;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        var pos = transform.position;
        pos.x -= Time.deltaTime/2;
        pos.y += Mathf.Cos(timer) * Time.deltaTime;　//微分すると変化量がCosなので、サインカーブを描けます。
        transform.position = pos;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if((other).tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(-healNum);
            Destroy(this.gameObject);
        }
    }
}
