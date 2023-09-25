using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    public Story4Manager story4Manager;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            story4Manager.Clear();
        }
    }
}
