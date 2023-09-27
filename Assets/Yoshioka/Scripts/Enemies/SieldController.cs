using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SieldController : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHp;
    private int hp;

    private SpriteRenderer spriteRenderer;

    public delegate void OnSieldBreakDelegate();
    public OnSieldBreakDelegate OnSieldBreak;

    void Start()
    {
        hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AddDamage(int damage, bool obstacle = false)
    {
        hp -= damage;

        spriteRenderer.color = new Color(0.5f * (1.0f-(float)hp/(float)maxHp),1,1,0.6f);

        if(hp<0)
        {
            OnSieldBreak();
            Destroy(this.gameObject);
        }
    }
}
