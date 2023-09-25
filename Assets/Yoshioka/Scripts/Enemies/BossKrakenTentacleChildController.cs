using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKrakenTentacleChildController : MonoBehaviour ,IDamagable
{
    [SerializeField] private BossKrakenTentacleController parentTentacle;

    public void AddDamage(int damage)
    {
        parentTentacle.AddDamage(damage);
    }
}
