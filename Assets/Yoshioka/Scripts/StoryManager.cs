using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoryManager : MonoBehaviour
{
    public GameObject prologueObj, Story1Obj, Story2Obj, Story3Obj;
    private int currentStoryNum;

    void Start()
    {
        List<Flag> clearFlags = new List<Flag>();

        Flag[] flags = PlayerDataManager.Instance.GetFlags();

        //Flag名にClearとついているものを抜き取る
        foreach(Flag flag in flags)
        {
            if(flag.name.Contains("Clear"))
            {
                clearFlags.Add(flag);
            }
        }

        //Prologueという名前がついたclearFlagがあれば、Story1のロックを解除する。
        foreach(Flag clearFlag in clearFlags)
        {
            if(clearFlag.name.Contains("Prologue"))
            {
                Story1Obj.transform.Find("LockedCover").gameObject.SetActive(!clearFlag.isClear);
                Story1Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = clearFlag.isClear;
            }
        }

        foreach(Flag clearFlag in clearFlags)
        {
            if(clearFlag.name.Contains("Story1"))
            {
                Story2Obj.transform.Find("LockedCover").gameObject.SetActive(!clearFlag.isClear);
                Story2Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = clearFlag.isClear;
            }
        }

        foreach(Flag clearFlag in clearFlags)
        {
            if(clearFlag.name.Contains("Story2"))
            {
                Story3Obj.transform.Find("LockedCover").gameObject.SetActive(!clearFlag.isClear);
                Story3Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = clearFlag.isClear;
            }
        }
    }
}
