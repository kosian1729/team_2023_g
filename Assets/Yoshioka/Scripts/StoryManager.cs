using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoryManager : MonoBehaviour
{
    public GameObject prologueObj, Story1Obj, Story2Obj, Story3Obj, Story4Obj, Story5Obj;
    private int currentStoryNum;

    void Start()
    {
        if(PlayerDataManager.Instance.GetFlag("isClear_Prologue"))
        {
            Story1Obj.transform.Find("LockedCover").gameObject.SetActive(false);
            Story1Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = true;
        }

        if(PlayerDataManager.Instance.GetFlag("isClear_Story1"))
        {
            Story2Obj.transform.Find("LockedCover").gameObject.SetActive(false);
            Story2Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = true;
        }

        if(PlayerDataManager.Instance.GetFlag("isClear_Story2"))
        {
            Story3Obj.transform.Find("LockedCover").gameObject.SetActive(false);
            Story3Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = true;
        }

        if(PlayerDataManager.Instance.GetFlag("isClear_Story3"))
        {
            Story4Obj.transform.Find("LockedCover").gameObject.SetActive(false);
            Story4Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = true;
        }

        if(PlayerDataManager.Instance.GetFlag("isClear_Story4"))
        {
            Story5Obj.transform.Find("LockedCover").gameObject.SetActive(false);
            Story5Obj.transform.Find("Play").gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
