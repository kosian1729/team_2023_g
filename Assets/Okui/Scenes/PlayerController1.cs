using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController1 : MonoBehaviour, IDamagable
{
    private Camera cam;
    private GameObject animPosObj;

    [Header("Player�̈ړ����x")]
    [SerializeField] private float playerSpeed;

    [Header("Player�̗̑�")]
    [SerializeField] private int maxHp;

    private int hp;�@//Player�̌���HP

    private float timer;�@//�e�̔��ˊԊu�p�̃^�C�}�[

    [Header("���ˈʒu")]
    [SerializeField] private float offset_x;

    [Header("UI�̃n�[�g�𐧌䂷��X�N���v�g")]
    public HeartManager heartManager;

    [Header("Bullet���Ǘ�����X�N���v�g")]
    public BulletManager bulletManager;

    [Header("GameOver�C�x���g")]
    public GameEvent GameOver;

    [Header("Boss�C�x���g")]
    public GameEvent Boss;

    public Animator animator;

    private Image damagePanel;

    private bool stop;

    private bool noDamageMode;

    void Start()
    {
        cam = Camera.main;
        animPosObj = transform.parent.gameObject;

        damagePanel = GameObject.Find("DamagePanel").GetComponent<Image>();
        damagePanel.color = Color.clear;

        hp = maxHp;
        heartManager.SetHeart(maxHp, hp);
        //animator = transform.parent.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (stop) return;

        Move();
        Attack();
        Select();

        //���G��ԃt���O��True�̂Ƃ��ɖ��t���[�����s
        if (isInvincible)
        {
            //�����ɖ��G��Ԃ̂Ƃ��̏���������
            Debug.Log("���G���");

            //���t���[���^�C�}�[�ϐ���Time.deltaTime�𑫂�
            invincibilityTimer += Time.deltaTime;

            //�^�C�}�[�����G����(10�b)�𒴂����Ƃ�
            if (invincibilityTimer >= invincibilityDuration)
            {
                Debug.Log("���G��ԏI���");

                //���G��ԃt���O��False�ɂ���
                isInvincible = false;
                //�^�C�}�[��0.0�b�Ƀ��Z�b�g����
                invincibilityTimer = 0.0f;
            }
        }
    }

    //Player���A���͂ɉ����������ւƈړ�������B
    void Move()
    {
        //WASD�����͂����ƁA-1~1�̐����l���Ԃ����B�i�����w��p�j
        float x = Input.GetAxisRaw("AD");
        float y = Input.GetAxisRaw("WS");

        //�J�����̒[�𒴂��Ă��鎞�A�v���C���[���͂ݏo�Ȃ��悤�ɂ���B
        var currentPos = transform.localPosition + new Vector3(x * Time.deltaTime * playerSpeed, y * Time.deltaTime * playerSpeed);
        var gap = animPosObj.transform.localPosition;

        currentPos.y = Mathf.Clamp(currentPos.y, -cam.orthographicSize - gap.y, cam.orthographicSize - gap.y);
        currentPos.x = Mathf.Clamp(currentPos.x, -cam.orthographicSize * 1920 / 1080 - gap.x, cam.orthographicSize * 1920 / 1080 - gap.x);

        transform.localPosition = currentPos;

    }

    void Attack()
    {
        // �X�y�[�X�L�[�������Ă���ԁA���Ԋu��bullet��ł�������
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) && (timer <= 0.0f) && (bulletManager.GetBulletNum() > 0))
        {
            AudioManager.Instance.PlaySE("SE�U��");

            Instantiate(bulletManager.GetBulletObj(), new Vector3(transform.position.x + offset_x, transform.position.y), Quaternion.Euler(0, 0, -90));
            bulletManager.ChangeBulletNum(-1, bulletManager.GetSlotNum());
            timer = bulletManager.GetBulletInterval(); // �Ԋu���Z�b�g
        }
        // �^�C�}�[�̒l�����炷
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bulletManager.ChangeSlotNum(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bulletManager.ChangeSlotNum(1);
        }
    }

    public float invincibilityDuration = 1.0f; // ���G���ԁi�b�j
    private float invincibilityTimer = 0.0f;   // �o�ߎ��Ԃ��i�[����^�C�}�[�ϐ�(�����l0�b)
    private bool isInvincible = false;         // ���G��Ԃ��ǂ����̃t���O

    //��_����
    public void AddDamage(int damage,bool obstacle = false)
    {
        if (!obstacle)
        {
            if (noDamageMode) return;
            if (isInvincible) return;
        }
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, hp);

        heartManager.SetHeart(maxHp, hp);

        damagePanel.color = new Color(0.8f, 0f, 0f, 0.8f);
        damagePanel.DOFade(0, 0.3f).SetEase(Ease.InQuad);


        if (hp <= 0)
        {
            //GameOver����
            GameOver.Raise();

            this.gameObject.SetActive(false);
        }

        {
            //�G�̒e�ɓ����������ɖ��G��ԃt���O��True�ɂ���
            isInvincible = true;
        }

    }

    public void StopControll(bool isStop)
    {
        stop = isStop;
        noDamageMode = isStop;

    }

    public void BeforeStageStart()
    {
        animator.SetTrigger("PlayerIN");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BossArea")
        {
            Boss.Raise();
        }
    }
}
