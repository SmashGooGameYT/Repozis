using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AIbanditL : MonoBehaviour
{
    public Animator animEnemy;
    private Rigidbody2D rb;

    public int maxHP = 100;
    int HPnow;

    // part 2

    [SerializeField] private BoxCollider2D CombatZone;
    [SerializeField] public LayerMask PlayerLayer;
    [SerializeField] Transform PlayerPos;

    public Transform DamageZone;
    public float RangePrepare = 0.5f;
    private int _damage = 12;

    void Start()
    {
        HPnow = maxHP;
    }

    void Update()
    {

    }


    public void TakeDamage(int damage)
    {
        HPnow -= damage;

        //Анимация ранения
        animEnemy.SetTrigger("Hurt");

        if (HPnow <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        //Анимация смерти
        animEnemy.SetBool("Die?", true);

        //Уничтожение
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }


    // part 2
    public void InPlayerCombatZone()
    {
        animEnemy.SetBool("PlayerInCombatZone", true);
    }
    public void NoPlayerCombatZone()
    {
        animEnemy.SetBool("PlayerInCombatZone", false);
    }

    void DamageAttack()
    {
        Collider2D[] HitPlayer = Physics2D.OverlapCircleAll(PlayerPos.position, RangePrepare, PlayerLayer);
        foreach (Collider2D PlayerHP in HitPlayer)
        {
            PlayerHP.GetComponent<HPScript>().TakeDamage(_damage);
        }
    }



    void OnDrawGizmosSelected()
    {
        if (DamageZone == null)
            return;

        Gizmos.DrawWireSphere(DamageZone.position, RangePrepare);
    }
}