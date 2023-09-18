using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    public GameEvent Boss;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "SpawnArea")
        {
            Boss.Raise();
        }
    }
}
