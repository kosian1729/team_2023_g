using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    [Header("表示したいテキスト")]
    [TextArea]
    public string text;

    [Header("表示したい名前")]
    public string name;

    [Header("表示したい立ち絵")]
    public Sprite sprite;

    [Header("立ち絵を右に表示")]
    public bool isRight;
}
