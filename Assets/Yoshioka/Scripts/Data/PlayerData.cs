using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData : ISerializationCallbackReceiver
{
    Dictionary<string,bool> flag = new Dictionary<string,bool>();

    //Dictionaryデータ保存用
    [SerializeField]
    List<string> keys = new List<string>();

    [SerializeField]
    List<bool> values = new List<bool>();

    string[] defaultKey = {
        "isClear_Prologue",
        "isClear_Story1",
        "isClear_Story2",
        "isClear_Story3",
        "isClear_Story4",
        "isClear_Story5",
        "isBoss_Story1",
        "isBoss_Story2",
        "isBoss_Story3",
        "isBoss_Story4",
        "isBoss_Story5"
        };

    bool[] defaultValues = {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
        };

    public void InitData()
    {
        keys.Clear();
        keys.AddRange(defaultKey);
        values.Clear();
        values.AddRange(defaultValues);

        flag = new Dictionary<string, bool>();
         // Math.Minで最小値を抽出
        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
        {
            flag.Add(keys[i], values[i]);
        }
    }


    // // ToJsonでクラスデータをJsonに変換する前
    public void OnBeforeSerialize()
    {
        Debug.Log("OnBeforeSerialize");
        keys.Clear();
        values.Clear();
        foreach (var f in flag)
        {
            keys.Add(f.Key);
            values.Add(f.Value);
        }
    }

    //// FromJsonでクラスデータを読み込んだ後
    public void OnAfterDeserialize()
    {
        Debug.Log("OnAfterDeserialize");
        flag = new Dictionary<string, bool>();
         // Math.Minで最小値を抽出
        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
        {
            flag.Add(keys[i], values[i]);
        }
    }

    public void SetFlag(string flagName, bool state)
    {
        flag[flagName] = state;
    }

    public bool GetFlag(string flagName)
    {
        return flag[flagName];
    }
}
