using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UniAttackController : MonoBehaviour
{
    public float bulletSpeed;
    private float timer;
    public int attackNum;
    private Quaternion initialRotation;
    public GameObject bullet;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,0,5.0f));

        if(bulletSpeed == 0.0f) return;

        timer += Time.deltaTime;
        transform.position += initialRotation * new Vector3(0,bulletSpeed,0)* Time.deltaTime;
        bulletSpeed = Mathf.Lerp(bulletSpeed,0.0f,Time.deltaTime * 0.6f);

        if(bulletSpeed <= 0.5f)
        {
            bulletSpeed = 0.0f;
            AttackPhase();
        }
    }

    public void AttackPhase()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOScale(new Vector3(0.8f,0.8f,0.8f), 1.0f)) //0-1.0
                .Append(this.transform.DOScale(new Vector3(1.1f,1.1f,1.1f),0.4f)).SetEase(Ease.InExpo)   //1.0-1.4
                .AppendCallback(() => {
                    Destroy(this.gameObject);
                });

        sequence.InsertCallback(1.2f,() =>{
            Attack();
        });
    }

    public void Attack()
    {
        float attackAngle;
        float offset = Random.Range(0.0f,360/attackNum);

        for(int n = 0; n<attackNum; n++)
        {
            attackAngle = (360/attackNum) * n + offset;
            Instantiate(bullet,this.transform.position,Quaternion.Euler(0,0,attackAngle));
        }
    }
}
