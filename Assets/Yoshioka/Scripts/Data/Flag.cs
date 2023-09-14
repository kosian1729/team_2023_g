using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Flag : ScriptableObject
{
    [Header("フラグ名")]
    public string name;
    [Header("Clearしているかのフラグ")]
    public bool isClear;
}
