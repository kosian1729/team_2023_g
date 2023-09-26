using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BossMachineController : MonoBehaviour, IDamagable
{
    public int maxHp;
    private int hp;
    public int hitPower;
    private bool isRage;
    private float timer;
    private float itemTimer;

    public Slider hpBar;
    public GameEvent AfterBoss;

    public GameObject sield;
    private bool isSield;

    public GameObject homingBullet;

    public GameObject missileBullet;

    public GameObject uniAttackBullet;

    public GameObject miniMachine;

    public GameObject beam;
    private BoxCollider2D beamCol;

    public Transform bulletOffset;
    public Transform sieldOffset;
    public Transform summonOffset;
    public Transform itemOffset;

    public List<GameObject> itemList = new List<GameObject>();
    private List<GameObject> _itemList = new List<GameObject>();

    private Animator anim;

    void Start()
    {
        hp = maxHp;
        hpBar.value = hp;

        beamCol = beam.GetComponent<BoxCollider2D>();
        beam.SetActive(false);

        anim = GetComponent<Animator>();

        _itemList = new List<GameObject>(itemList);

        this.enabled = false;
    }

    public void BossStart()
    {
        anim.SetInteger("Level",-1);
        timer = 21.0f;
    }

    //Attack animから呼ばれる
    public void HomingAttack()
    {
        Instantiate(homingBullet,bulletOffset.position,Quaternion.Euler(0,0,164));
    }

    public void MissileAttack(float rotationSpeed)
    {
        var _missile = Instantiate(missileBullet,bulletOffset.position,Quaternion.Euler(0,0,90));
        _missile.GetComponent<MissileController>().SetRotationSpeed(rotationSpeed);
    }

    public void UniAttack(float rotate)
    {
        Instantiate(uniAttackBullet,bulletOffset.position,Quaternion.Euler(0,0,rotate));
    }

    public void SummonSield()
    {
        isSield = true;
        var _sield  = Instantiate(sield,sieldOffset.position,Quaternion.identity,sieldOffset);
        _sield.GetComponent<SieldController>().OnSieldBreak =()=> {
            isSield = false;
        };
    }

    public void SummonMiniMachine()
    {
        Instantiate(miniMachine,summonOffset.position + new Vector3(0,3.0f,0),Quaternion.identity);
        Instantiate(miniMachine,summonOffset.position + new Vector3(-1.3f,-2.3f,0),Quaternion.identity);
    }

    public void SummonItem()
    {
        if(_itemList.Count == 0)
        {
            _itemList = new List<GameObject>(itemList);
        }

        int index = UnityEngine.Random.Range(0,_itemList.Count);

        Instantiate(_itemList[index],itemOffset.position,Quaternion.identity);
        _itemList.RemoveAt(index);
    }

    public void OnEndAnimationStateMachine()
    {
        anim.SetTrigger("EndStateMachine");

        if(hp<maxHp/2 && !isRage)
        {
            isRage = true;
            anim.SetInteger("Level",5);
        }
        else
        {
           anim.SetInteger("Level",SetLevel()) ;
        }
    }

    int SetLevel()
    {
        int level;

        if(hp>maxHp * 0.9f)
        {
            level = UnityEngine.Random.Range(1,3);
        }
        else if(hp>maxHp * 0.8f)
        {
            level = UnityEngine.Random.Range(2,4);
        }
        else if(hp>maxHp * 0.7f)
        {
            level = UnityEngine.Random.Range(1,5);
        }
        else if(hp>maxHp * 0.6f)
        {
            level = UnityEngine.Random.Range(2,5);
        }
        else if(hp>maxHp * 0.5f)
        {
            level = UnityEngine.Random.Range(1,5);
        }
        else if(hp>maxHp * 0.4f)
        {
            level = UnityEngine.Random.Range(6,8);
        }
        else if(hp>maxHp * 0.3f)
        {
            level = UnityEngine.Random.Range(7,9);
        }
        else if(hp>maxHp * 0.2f)
        {
            level = UnityEngine.Random.Range(6,10);
        }
        else if(hp>maxHp * 0.1f)
        {
            level = UnityEngine.Random.Range(8,10);
        }
        else if(hp>0)
        {
            level = UnityEngine.Random.Range(6,10);
        }
        else 
        {
            level = 1;
        }
        
        return level;
    }

    public void AddDamage(int damage, bool obstacle = false)
    {
        if(isSield) return;

        hp -= damage;
        hp = Mathf.Clamp(hp,0,maxHp);
        hpBar.value = hp;

        if(hp<=0)
        {
            AfterBoss.Raise();
            Destroy(this.gameObject);
        }
    }

    //プレイヤーとの衝突時の処理
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(hitPower);
        }
    }

    public void StartBeam()
    {
        beam.SetActive(true);
        beamCol.enabled = false;

        //0.6f後から当たり判定をつける
        Invoke(new Action(()=> {
            beamCol.enabled = true;
        }).Method.Name,0.6f);
    }

    public void EndBeam()
    {
        beam.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer>30.0f)
        {
            SummonSield();
            timer = 0.0f;
        }

        itemTimer += Time.deltaTime;
        if(itemTimer > 37.0f)
        {
            SummonItem();
            itemTimer = 0.0f;
        }
    }
}