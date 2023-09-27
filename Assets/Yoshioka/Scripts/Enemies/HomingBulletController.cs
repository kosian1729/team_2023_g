using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBulletController : MonoBehaviour
{
    [Header("弾速")]
    [SerializeField] private float bulletSpeed;

    public float rotationSpeed = 4.0f; 

    [Header("威力")]
    [SerializeField] private int power;

    [Header("弾が消えるまでの時間")]
    [SerializeField] private float duration;

    private float timer;　//弾の生存時間

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Destroy(this.gameObject,duration);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer>0.4f)
        {
            Vector2 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            bulletSpeed -= 2.0f * Time.deltaTime;
            bulletSpeed = Mathf.Clamp(bulletSpeed,3.5f,bulletSpeed);

            rotationSpeed -= 1.0f * Time.deltaTime;
            rotationSpeed = Mathf.Clamp(rotationSpeed,0,rotationSpeed);
        }

        transform.position += transform.rotation * new Vector3(bulletSpeed,0,0)* Time.deltaTime;


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(power);
            Destroy(this.gameObject);
        }

    }
}
