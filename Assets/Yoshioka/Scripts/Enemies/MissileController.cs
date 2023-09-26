using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [Header("弾速")]
    [SerializeField] private float bulletSpeed;

    public float rotationSpeed = 1.0f; 

    [Header("威力")]
    [SerializeField] private int power;

    [Header("弾が消えるまでの時間")]
    [SerializeField] private float duration;

    public GameObject explosionParticle;

    private Camera cam;

    private Vector2 direction;

    private float timer;　//弾の生存時間

    void Start()
    {
        cam = Camera.main;
        Destroy(this.gameObject,duration);
    }

    public void SetRotationSpeed(float _rotationSpeed)
    {
        rotationSpeed = _rotationSpeed;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if(timer <= 2.0f)
        {
            bulletSpeed -= 2.0f * Time.deltaTime;   
        }
        else if(timer > 2.0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,270),rotationSpeed);
        }

        if(transform.position.y < -cam.orthographicSize - 0.3f)
        {
            Instantiate(explosionParticle,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }

        transform.position += transform.rotation * new Vector3(bulletSpeed,0,0)* Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(power);
            Instantiate(explosionParticle,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
