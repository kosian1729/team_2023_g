using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataManager : SingletonMonoBehaviour<PlayerDataManager>
{
    [SerializeField]
    private PlayerData playerData;

    public void Awake()
    {
        Load();
        Application.quitting += Save;
    }

    public void Save()
    {
        playerData.Save();
    }

    public void Load()
    {
        playerData.Load();
    }

    public Flag[] GetFlags()
    {
        return playerData.flags;
    }

    public Flag GetFlag(int num)
    {
        return playerData.flags[num];
    }

    public void SetIsClear(string _name, bool _isClear)
    {
        Flag flag = Array.Find(playerData.flags,_flag => _flag.name == _name);

        if(flag == null)
        {
            Debug.Log(flag.name + "という名前のフラグは存在しません。");
            return;
        }

        flag.isClear = _isClear;
    }
}
