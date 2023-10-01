using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour, IDamagable
{
    [Header("�G�̃X�s�[�h")]
    [Tooltip("�W���l��1�ł��B")]
    [SerializeField] private float speed;

    [Header("�G�̗̑�")]
    [SerializeField] private int hp;

    [Header("�G�̍U���́i�Փˎ��j")]
    [SerializeField] private int hitPower;

    [Header("�G�̍U���e")]
    [SerializeField] private GameObject bullet;

    [Header("�U���Ԋu")]
    [SerializeField] private float interval;

    [Header("�v���C���[���˂���đł�")]
    [SerializeField] private bool isAim;

    private Transform player;

    private int phase;

    private float timer;    //�X�|�[�����Ă���̌o�ߎ���
    private int num; //�U�����邽�тɉ��Z

    void Start()
    {
        phase = 1;
        num = 1;

        if (hp <= 0.0f)
        {
            Debug.Log(this.gameObject.name + "�̗̑͂��ݒ肳��Ă��Ȃ����A0�����ł��B");
            Destroy(this.gameObject);
        }

        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        timer += Time.deltaTime;
        var pos = transform.position;

        switch (phase)
        {
            case 1:
                pos.x -= Time.deltaTime;
                pos.y += Mathf.Cos(timer) * Time.deltaTime; //��������ƕω��ʂ�Cos�Ȃ̂ŁA�T�C���J�[�u��`���܂��B

                if (timer > 1.8f)
                {
                    phase = 2;
                }
                break;

            case 2:
                pos.x += Time.deltaTime;
                pos.y += Mathf.Cos(timer) * Time.deltaTime; //��������ƕω��ʂ�Cos�Ȃ̂ŁA�T�C���J�[�u��`���܂��B

                if (timer > 1.8f + interval * num)
                {
                    Attack();
                    num++;
                }
                break;
        }

        transform.position = pos;
    }

    void Attack()
    {
        if(player == null) return;

        //player���������E�ɍs���ƍU������߂�
        if (this.transform.position.x < player.position.x) return;

        if (isAim)
        {
            Vector3 direction = player.position - this.transform.position;
            Instantiate(bullet, this.transform.position, Quaternion.FromToRotation(Vector3.up, direction));
        }
        else
        {
            Instantiate(bullet, this.transform.position, Quaternion.Euler(0, 0, Random.Range(85.0f, 95.0f)));
        }

    }



    //�v���C���[�̒e�ɓ��������Ƃ��Ăяo�����
    public void AddDamage(int damage, bool obstacle = false)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //���S���̉��o�Ȃǂ�����Ȃ炱��
            Destroy(this.gameObject);
        }
    }

    //�v���C���[�Ƃ̏Փˎ��̏���
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(hitPower);
        }
    }
}

