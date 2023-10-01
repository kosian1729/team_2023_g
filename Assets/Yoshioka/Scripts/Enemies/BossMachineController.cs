using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BossMachineController : MonoBehaviour, IDamagable
{
    public int maxHp;
    private int hp;

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

    public GameObject explosionObj;

    public GameObject miniMachine;

    public GameObject energyObj;
    private GameObject _energyObj;

    public GameObject beam;
    private BoxCollider2D beamCol;

    public Transform bulletOffset;
    public Transform sieldOffset;
    public Transform summonOffset;
    public Transform itemOffset;
    public Transform[] explosionOffsets;

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
        timer = 26.0f;
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

    IEnumerator ExplosionCotoutine()
    {
        var exObj_1 = Instantiate(explosionObj,explosionOffsets[0].position,Quaternion.identity);
        exObj_1.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
        yield return new WaitForSeconds(0.4f);

        var exObj_2 = Instantiate(explosionObj,explosionOffsets[UnityEngine.Random.Range(1,3)].position,Quaternion.identity);
        exObj_2.transform.localScale = new Vector3(0.9f,0.9f,0.9f);
        yield return new WaitForSeconds(0.4f);

        var exObj_3 = Instantiate(explosionObj,explosionOffsets[UnityEngine.Random.Range(3,6)].position,Quaternion.identity);
        exObj_3.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
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

    public void SummonEnergy()
    {
        if(_energyObj == null)
        {
            _energyObj = Instantiate(energyObj,transform.position,Quaternion.identity);
        }
        else
        {
            _energyObj.GetComponent<EnergyController>().StartCoroutine("TripleEnergyAttack");
        }
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
            if(_energyObj!=null)
            {
                _energyObj.GetComponent<IDamagable>().AddDamage(100);
            }

            anim.enabled = false;
            var endEx1 = Instantiate(explosionObj,transform.position,Quaternion.identity);
            endEx1.transform.localScale = new Vector3(3.0f,3.0f,3.0f);

            var endSequence = DOTween.Sequence().SetLink(gameObject);

            endSequence.InsertCallback(0.8f,()=>{
                var endEx2 = Instantiate(explosionObj,transform.position + new Vector3(1.0f,1.0f,0.0f),Quaternion.identity);
                endEx2.transform.localScale = new Vector3(2.0f,2.0f,2.0f);
            });

            endSequence.InsertCallback(2.0f,()=>{
                var endEx2 = Instantiate(explosionObj,transform.position + new Vector3(0.0f,2.0f,0.0f),Quaternion.identity);
                endEx2.transform.localScale = new Vector3(4.5f,4.5f,4.5f);
                
            });

            endSequence.InsertCallback(2.3f,()=>{
                AfterBoss.Raise();
                Destroy(this.gameObject);
            });
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
