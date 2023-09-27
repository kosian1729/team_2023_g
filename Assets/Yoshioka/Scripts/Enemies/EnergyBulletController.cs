using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBulletController : MonoBehaviour
{
    [SerializeField] private int hitPower;
    private float X,Y;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        X = 0.9f * Time.deltaTime;
        Y = Mathf.Sin(timer * Mathf.PI/3) * 0.1f * Time.deltaTime;
        transform.position += transform.rotation * new Vector3(-X,Y);
    }
}
