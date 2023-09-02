using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<Summary>
///敵キャラのスポーンを制御します。
///</Summary>
public class Activator : MonoBehaviour
{
    private GameObject obj;

    void Awake()
    {
        obj = transform.GetChild(0).gameObject;
        obj.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "SpawnArea")
        {
            obj.SetActive(true);
            transform.DetachChildren();
            Destroy(this.gameObject);
        }
    }
}
