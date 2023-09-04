using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("カメラのスクロールスピード")]
    [SerializeField] private float scrollSpeed;
    private bool pouse;

    void Update()
    {
        if(pouse) return;
        //カメラのx座標を増加させてスクロールさせる
        this.gameObject.transform.position += new Vector3(Time.deltaTime * scrollSpeed,0);
    }

    public void PouseCameraScroll(bool _pouse)
    {
        pouse = _pouse;
    }

    public IEnumerator FadeIn(float seconds)
    {
        float oriSpeed = scrollSpeed;
        while(seconds>0)
        {
            scrollSpeed = oriSpeed/(seconds+1);
            seconds -= Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
        }
        scrollSpeed = oriSpeed;
    }

    public IEnumerator FadeOut()
    {
        float oriSpeed = scrollSpeed;
        for(int i=0; i<200; i++)
        {
            scrollSpeed = scrollSpeed/2;
            yield return new WaitForSeconds(0.02f);
        }
        
        PouseCameraScroll(true);
        scrollSpeed = oriSpeed;
    }
}
