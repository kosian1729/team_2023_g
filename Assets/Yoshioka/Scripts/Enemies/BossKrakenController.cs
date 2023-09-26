using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossKrakenController : MonoBehaviour, IDamagable
{
    [Header("敵の体力")]
    [SerializeField] private int maxHp;
    [SerializeField] private Sprite rageSprite; 
    [SerializeField] private GameObject summonEffect;
    [SerializeField] private GameObject rageSummonEffect;
    [SerializeField] private GameObject rageEffect;
    [SerializeField] private GameObject warpEffect;
    [SerializeField] private GameObject tentacle; 
    [SerializeField] private GameObject rageTentacle;
    [SerializeField] private GameEvent AfterBoss;
    [Header("HPバー")]
    public Slider hpBar;
 
    [SerializeField] private float bossCameraPos_x;

    private int hp;

    [Header("敵の攻撃力（衝突時）")]
    [SerializeField] private int hitPower;

    private int phase = 0;
    private int mPhase = 0;
    private float timer;
    private float mTimer;
    private Transform player;
    private GameObject despawnArea;
    private bool isRage;
    private SpriteRenderer spriteRenderer;

    Sequence sequence_1;
    Sequence sequence_2;
    Sequence sequence_3;

    public void BossStart()
    {
        phase = 1;
        mPhase = 1;
    }

    void Start()
    {
        hp = maxHp;
        hpBar.value = hp;
        isRage = false;
        player = GameObject.FindWithTag("Player").transform;
        despawnArea = GameObject.FindWithTag("DespawnArea");
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0;
        mTimer = 0;
        despawnArea.SetActive(false);

        transform.position = new Vector3(bossCameraPos_x +15.0f, -1.0f);
    }

    public void AddDamage(int damage, bool obstacle = false)
    {
        if(isRage) damage /= 2;
        hp -= damage;
        hp = Mathf.Clamp(hp,0,hp);
        hpBar.value = hp;

        if(hp<=0)
        {
            AfterBoss.Raise();
            //死亡時の演出などをつけるならここ
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<IDamagable>().AddDamage(hitPower);
        }
    }

    void Update()
    {
        TentacleAttack();
        Move();

        if(hp<=maxHp/2 && isRage == false)
        {
            isRage = true;
        }
    }

    void TentacleAttack()
    {

        if(phase == 0) return;
        timer += Time.deltaTime;        

        switch(phase)
        {
            case 1:
                if(timer>4.0f)
                {
                    phase = 2;
                    StartCoroutine(Summon(bossCameraPos_x + Random.Range(-5.0f,-1.5f), 1 + Random.Range(-1,1)*2));
                    StartCoroutine(Summon(bossCameraPos_x + Random.Range(1.5f,5.0f), 1 + Random.Range(-1,1)*2));
                    timer = 0;
                }
                break;

            case 2:
                sequence_1 = DOTween.Sequence();

                sequence_1.InsertCallback(4.0f,() =>{
                    StartCoroutine(Summon(player.position.x, 1 + Random.Range(-1,1)*2));
                });

                sequence_1.InsertCallback(4.4f,() =>{
                    StartCoroutine(Summon(player.position.x+2, 1 + Random.Range(-1,1)*2));
                });

                sequence_1.InsertCallback(4.8f,() =>{
                    StartCoroutine(Summon(player.position.x+4, 1 + Random.Range(-1,1)*2));
                });

                sequence_1.InsertCallback(5.2f,() =>{
                    phase = 3;
                });

                phase = 0;
                break;

            case 3:
                sequence_2 = DOTween.Sequence();

                sequence_2.InsertCallback(4.0f,() =>{
                    StartCoroutine(Summon(player.position.x, 1 + Random.Range(-1,1)*2));
                });

                sequence_2.InsertCallback(4.4f,() =>{
                    StartCoroutine(Summon(player.position.x-2, 1 + Random.Range(-1,1)*2));
                });

                sequence_2.InsertCallback(4.8f,() =>{
                    StartCoroutine(Summon(player.position.x-4, 1 + Random.Range(-1,1)*2));
                });

                sequence_2.InsertCallback(5.2f,() =>{
                    phase = 4;
                });

                phase = 0;
                break;

            case 4:
                sequence_3 = DOTween.Sequence();

                sequence_3.InsertCallback(3.0f,() =>{
                    StartCoroutine(Summon(player.position.x + Random.Range(-5.0f,-2.0f), 1 + Random.Range(-1,1)*2));
                    StartCoroutine(Summon(player.position.x + Random.Range(0.0f,3.0f), 1 + Random.Range(-1,1)*2));
                });

                sequence_3.InsertCallback(5.0f,() =>{
                    StartCoroutine(Summon(player.position.x + Random.Range(-5.0f,-2.0f), 1 + Random.Range(-1,1)*2));
                    StartCoroutine(Summon(player.position.x + Random.Range(0.0f,3.0f), 1 + Random.Range(-1,1)*2));
                });

                sequence_3.InsertCallback(7.0f,() =>{
                    StartCoroutine(Summon(bossCameraPos_x + Random.Range(-7.5f,-4.5f), 1 + Random.Range(-1,1)*2));
                    StartCoroutine(Summon(bossCameraPos_x + Random.Range(-3.5f,-0.5f), 1 + Random.Range(-1,1)*2));
                    StartCoroutine(Summon(bossCameraPos_x + Random.Range(1.5f,3.5f), 1 + Random.Range(-1,1)*2));
                    StartCoroutine(Summon(bossCameraPos_x + Random.Range(4.5f,7.5f), 1 + Random.Range(-1,1)*2));
                });

                sequence_3.InsertCallback(8.0f,() =>{
                    phase = 1;
                });

                phase = 0;
                break;

            case -1:
                phase = 0;
                
                StartCoroutine(RandomRageSummon());
                break;

            case -2:
                phase = 0;
                var sequence_H = DOTween.Sequence();

                sequence_H.InsertCallback(2.0f,() =>{
                    StartCoroutine(HorizonRageSummon(1 , -2.2f));
                    StartCoroutine(HorizonRageSummon(-1 , 2.2f));
                });
                sequence_H.InsertCallback(3.0f,() =>{
                    StartCoroutine(HorizonRageSummon(1 , 0));
                    StartCoroutine(HorizonRageSummon(-1 , -2.2f));
                });
                sequence_H.InsertCallback(4.0f,() =>{
                    StartCoroutine(HorizonRageSummon(1 , 2.2f));
                    StartCoroutine(HorizonRageSummon(-1 , 0));
                });
                sequence_H.InsertCallback(6.0f,() =>{
                    StartCoroutine(HorizonRageSummon(1 , -2.2f));
                    StartCoroutine(HorizonRageSummon(-1 , 0));
                });
                sequence_H.InsertCallback(7.0f,() =>{
                    StartCoroutine(HorizonRageSummon(-1 , -2.2f));
                    StartCoroutine(HorizonRageSummon(1 , 2.2f));
                    mPhase = -2;
                });
                break;
        }
    }

    void Move()
    {
        if(mPhase == 0) return;
        mTimer += Time.deltaTime;
        var pos = transform.position;

        switch(mPhase)
        {
            case 1:
                pos.x -= Time.deltaTime * 2.0f;
                pos.y += Mathf.Sin(mTimer*Mathf.PI/2) * Time.deltaTime *2.0f;　//微分すると変化量がCosなので、サインカーブを描けます。

                transform.position = pos;

                if(mTimer>16.0f)
                {
                    mTimer = 0;
                    mPhase = 2;
                    transform.position = new Vector3(bossCameraPos_x-10.7f,-8.9f);
                    transform.rotation = Quaternion.Euler(0,180,0);

                    if(isRage)
                    {
                        mPhase = -1;
                    }
                }
                break;

            case 2:
                pos.x += Time.deltaTime * 2.0f;
                pos.y += Mathf.Sin(mTimer*Mathf.PI/2) * Time.deltaTime *2.0f;　//微分すると変化量がCosなので、サインカーブを描けます。

                transform.position += Quaternion.Euler(0,0,40f) * new Vector3(Time.deltaTime * 2.0f,Mathf.Sin(mTimer*Mathf.PI/2) * Time.deltaTime *2.0f);

                if(mTimer>16.0f)
                {
                    mTimer = 0;
                    mPhase = 4;
                    transform.position = new Vector3(bossCameraPos_x - 15.0f, -1.0f);

                    if(isRage)
                    {
                        mPhase = -1;
                    }
                }
                break;

            case 3:
                pos.x -= Time.deltaTime * 2.0f;
                pos.y += Mathf.Sin(mTimer*Mathf.PI/2) * Time.deltaTime *2.0f;　//微分すると変化量がCosなので、サインカーブを描けます。

                transform.position += Quaternion.Euler(0,0,140f) * new Vector3(Time.deltaTime * 2.0f,Mathf.Sin(mTimer*Mathf.PI/2) * Time.deltaTime *2.0f);

                if(mTimer>16.0f)
                {
                    mTimer = 0;
                    mPhase = 1;
                    transform.position = new Vector3(bossCameraPos_x + 15.0f, -1.0f);

                    if(isRage)
                    {
                        mPhase = -1;
                    }
                }
                break;

            case 4:
                pos.x += Time.deltaTime * 2.0f;
                pos.y += Mathf.Sin(mTimer*Mathf.PI/2) * Time.deltaTime *2.0f;　//微分すると変化量がCosなので、サインカーブを描けます。

                transform.position = pos;

                if(mTimer>16.0f)
                {
                    mTimer = 0;
                    mPhase = 3;
                    transform.position = new Vector3(bossCameraPos_x+10.7f,-8.9f);
                    transform.rotation = Quaternion.Euler(0,0,0);

                    if(isRage)
                    {
                        mPhase = -1;
                    }
                }
                break;

            case -1:
                mPhase = 0;
                phase = 0;

                sequence_1.Kill();
                Debug.Log("sequence is killed");
                sequence_2.Kill();
                sequence_3.Kill();

                transform.position = new Vector3(bossCameraPos_x, 10.7f);
                transform.rotation = Quaternion.Euler(0,0,0);
                
                var sequence_R = DOTween.Sequence();

                sequence_R.Append(transform.DOMoveY(0,4.0f))
                          .Append(transform.DOScale(new Vector3(1.25f,1.25f,1.25f),5.0f))
                          .Append(transform.DOScale(new Vector3(1.5f,1.5f,1.5f),0.5f))
                          .AppendInterval(0.5f)
                          .Append(transform.DOMoveY(10.7f,2.0f))
                          .AppendCallback(()=>{
                              phase = -1;
                          });

                sequence_R.InsertCallback(4.1f,() =>{
                    Instantiate(rageEffect,transform.position,Quaternion.identity);
                });

                sequence_R.InsertCallback(9.1f,() =>{
                    spriteRenderer.sprite = rageSprite;
                });
                break;

            case -2:
                mPhase = 0;
                transform.position = new Vector3(bossCameraPos_x + Random.Range(-13.0f,13.0f),10.7f*(1 + Random.Range(-1,1)*2));
                var sequence_W1 = DOTween.Sequence();

                sequence_W1.AppendInterval(0.9f)
                          .Append(transform.DOMove(new Vector3(bossCameraPos_x + Random.Range(-2.0f,2.0f),0),3.2f))
                          .AppendInterval(0.3f)
                          .Append(transform.DOScale(new Vector3(0.0f,0.0f,0.0f),0.15f))
                          .AppendCallback(()=>{
                            var _effect = Instantiate(warpEffect,transform.position,Quaternion.identity);
                            Destroy(_effect,3.0f);

                            transform.position = new Vector3(bossCameraPos_x,18.0f);
                            transform.DOScale(new Vector3(1.5f,1.5f,1.5f),0.1f);
                            mPhase = -3;
                          });

                sequence_W1.InsertCallback(0.35f,()=> {
                    StartCoroutine(HorizonRageSummon(1 + Random.Range(-1,1)*2, 0.9f * Random.Range(-3,4)));
                });

                sequence_W1.InsertCallback(0.95f,()=> {
                    StartCoroutine(RageSummon(bossCameraPos_x + 1.6f*Random.Range(-5,6), 1 + Random.Range(-1,1)*2, 3.5f));
                });
                break;

            case -3:
                mPhase = 0;
                transform.position = new Vector3(bossCameraPos_x + 15.0f*(1 + Random.Range(-1,1)*2), Random.Range(-7.0f,7.0f));
                var sequence_W2 = DOTween.Sequence();

                sequence_W2.AppendInterval(0.9f)
                          .Append(transform.DOMove(new Vector3(bossCameraPos_x, Random.Range(-1.0f,1.0f)),3.2f))
                          .AppendInterval(0.3f)
                          .Append(transform.DOScale(new Vector3(0.0f,0.0f,0.0f),0.15f))
                          .AppendCallback(()=>{
                            var _effect = Instantiate(warpEffect,transform.position,Quaternion.identity);
                            Destroy(_effect,3.0f);

                            transform.position = new Vector3(bossCameraPos_x,18.0f);
                            transform.DOScale(new Vector3(1.5f,1.5f,1.5f),0.1f);
                            mPhase = -2;
                          });
                
                sequence_W2.InsertCallback(0.35f,()=> {
                    StartCoroutine(RageSummon(bossCameraPos_x + 1.6f*Random.Range(-5,6), 1 + Random.Range(-1,1)*2, 3.5f));
                });

                sequence_W2.InsertCallback(0.95f,()=> {
                    StartCoroutine(HorizonRageSummon(1 + Random.Range(-1,1)*2, 0.9f * Random.Range(-3,4)));
                });
                break;              
        }

    }

    IEnumerator Summon(float X,float Y,float offset = 1.5f)
    {
        var _effect = Instantiate(summonEffect,new Vector3(X,-4.1f*Y),Quaternion.Euler(0,0,90 + 90*Y));
        Destroy(_effect,6.0f);

        yield return new WaitForSeconds(1.2f);

        Instantiate(tentacle,new Vector3(X, offset*Y),Quaternion.Euler(0,0,90 + 90*Y));
    }

    IEnumerator RageSummon(float X,float Y,float offset = 1.8f)
    {
        var _effect = Instantiate(rageSummonEffect,new Vector3(X,-4.1f*Y),Quaternion.Euler(0,0,90 + 90*Y));
        Destroy(_effect,6.0f);

        yield return new WaitForSeconds(1.0f);
        
        Instantiate(rageTentacle,new Vector3(X, offset*Y),Quaternion.Euler(0,0,90 + 90*Y));
    }

    IEnumerator HorizonRageSummon(float X,float Y,float offset = 2.4f)
    {
        var _effect = Instantiate(rageSummonEffect,new Vector3(bossCameraPos_x - 7.9f *X,Y),Quaternion.Euler(0,0,90*X));
        Destroy(_effect,6.0f);

        yield return new WaitForSeconds(1.0f);
        
        Instantiate(rageTentacle,new Vector3(bossCameraPos_x + offset*X, Y),Quaternion.Euler(0,0,90*X));
    }


    IEnumerator RandomRageSummon()
    {
        List<int> numbers = new List<int>(){-5,-4,-3,-2,-1,0,1,2,3,4,5};

        while(numbers.Count > 0)
        {
            int index = Random.Range(0,numbers.Count);

            StartCoroutine(RageSummon(bossCameraPos_x + 1.6f * numbers[index], 1 + Random.Range(-1,1)*2, 3.5f));

            numbers.RemoveAt(index);

            yield return new WaitForSeconds(0.45f);
        }

        phase = -2;
    }

}
