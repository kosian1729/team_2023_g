using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<Summary>
///敵キャラのスポーンを制御します。
///</Summary>
public class Activator : MonoBehaviour
{
    private EnemyController enemyController;

    void Awake()
    {
        enemyController = this.gameObject.GetComponent<EnemyController>();
        enemyController.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "SpawnArea")
        {
            enemyController.enabled = true;
        }
    }
}
