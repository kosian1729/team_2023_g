using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bulletの情報を格納するクラス
[CreateAssetMenu]
public class BulletInfo : ScriptableObject
{
    public string name;
    public GameObject obj;
    public Sprite sprite;
    public float interval;

    [HideInInspector]
    public int num;
}
