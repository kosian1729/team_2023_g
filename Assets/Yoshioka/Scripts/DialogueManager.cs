using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textZone;

    public Dialogue[] dialogueList;

    private int num;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            num++;
            textZone.text = dialogueList[num].dialogue;
        }
    }

}
