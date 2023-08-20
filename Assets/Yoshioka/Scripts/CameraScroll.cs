using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("カメラのスクロールスピード")]
    [SerializeField] private float scrollSpeed;

    void Update()
    {
        //カメラのx座標を増加させてスクロールさせる
        this.gameObject.transform.position += new Vector3(Time.deltaTime * scrollSpeed,0);
    }
}
