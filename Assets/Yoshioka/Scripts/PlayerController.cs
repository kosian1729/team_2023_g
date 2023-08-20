using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Playerの移動速度")]
    [SerializeField] private float playerSpeed;

    [Header("発射の間隔")]
    [SerializeField] private float interval;

    private float timer;　//弾の発射間隔用のタイマー

    [Header("発射位置")]
    [SerializeField] private float offset_x;

    [Header("通常弾のプレハブ")]
    [SerializeField] private GameObject normal_bullet;


    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        //WASDもしくは矢印キーが入力されると、-1~1の整数値が返される。（方向指定用）
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //Playerを、入力に応じた方向へと移動させる。
        this.gameObject.transform.position += new Vector3(x*Time.deltaTime*playerSpeed,y*Time.deltaTime*playerSpeed);
    }

    void Attack()
    {
        // スペースキーを押している間、一定間隔でbulletを打ち続ける
        if(Input.GetKey(KeyCode.Space) && timer <= 0.0f)　// 分岐条件変更
        {
            Instantiate(normal_bullet, new Vector3(transform.position.x + offset_x,transform.position.y), Quaternion.Euler(0,0,-90));
            timer = interval; // 間隔をセット
        }
        // タイマーの値を減らす
        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }
}
