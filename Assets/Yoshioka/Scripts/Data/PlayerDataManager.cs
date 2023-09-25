using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PlayerDataManager : SingletonMonoBehaviour<PlayerDataManager>
{
    private PlayerData playerData;

    public void Awake()
    {
        playerData = new PlayerData();
        Load();
    }

    public void Save()
    {
        var data = JsonUtility.ToJson(playerData);

        Debug.Log(data);
        PlayerPrefs.SetString("PlayerData", data);
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("PlayerData"))
        {
            var data = PlayerPrefs.GetString("PlayerData");

            Debug.Log(data);
            playerData = JsonUtility.FromJson<PlayerData>(data);        
        }
        else
        {
            Reset();
        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("PlayerData");
        playerData.InitData();
    }

    public bool GetFlag(string flagName)
    {
        Load();
        return playerData.GetFlag(flagName);
    }

    public void SetFlag(string flagName, bool state)
    {
        playerData.SetFlag(flagName,state);
        Save();
    }
}
