using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject 
{
    [Header("Flagを管理する")]
    public Flag[] flags;

    public void Save()
    {
        var data = JsonUtility.ToJson(this, true);

        PlayerPrefs.SetString("PlayerData", data);
    }

    public void Load()
    {
        if(!PlayerPrefs.HasKey("PlayerData")) return;
        
        var data = PlayerPrefs.GetString("PlayerData");

        JsonUtility.FromJsonOverwrite(data, this);
    }
}
