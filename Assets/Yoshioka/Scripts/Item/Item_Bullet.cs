using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bullet : MonoBehaviour
{
    public BulletManager bulletManager;
    private float timer;

    [Header("アイテム名（間違いがあると取得できないので注意")]
    [SerializeField] private string itemName;

    [Header("獲得数")]
    [SerializeField] private int num;


    void Start()
    {
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
    }

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
            Effect();
            Destroy(this.gameObject);
        }
    }

    void Effect()
    {
        if(bulletManager.GetBulletName(0)==itemName)
        {
            bulletManager.ChangeBulletNum(num,0);
        }
        else if(bulletManager.GetBulletName(1)==itemName)
        {
            bulletManager.ChangeBulletNum(num,1);
        }
        else if(bulletManager.GetBulletName(2)==itemName)
        {
            bulletManager.ChangeBulletNum(num,2);
        }
        else
        {
            Debug.Log("一致するアイテム名がありませんでした。");
        }
    }
}
