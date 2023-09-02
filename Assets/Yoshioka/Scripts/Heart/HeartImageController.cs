using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartImageController : MonoBehaviour
{
    [Header("ハートの画像の片割れ")]
    [Tooltip("0番:透明 1番:通常時 2番:ダメージを受けたとき")]
    [SerializeField] private Sprite[] HeartImages;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    //受け取ったidによって画像を変化させる
    public void ChangeHeartState(int id)
    {
        image.sprite = HeartImages[id];
    }
}
