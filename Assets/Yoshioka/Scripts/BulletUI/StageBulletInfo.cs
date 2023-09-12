using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageBulletInfo : ScriptableObject
{
    [Header("Stage名(Scene名)")]
    public string stageName;

    [Header("Slot0")]
    public BulletInfo bullet_0;
    [Tooltip("初期装弾数")]
    public int num_0;
    [Header("無限化")]
    public bool isInfinity_0;
    
    [Header("Slot1")]
    public BulletInfo bullet_1;
    [Tooltip("初期装弾数")]
    public int num_1;
    [Header("無限化")]
    public bool isInfinity_1;

    [Header("Slot2")]
    public BulletInfo bullet_2;
    [Tooltip("初期装弾数")]
    public int num_2;
    [Header("無限化")]
    public bool isInfinity_2;
}
