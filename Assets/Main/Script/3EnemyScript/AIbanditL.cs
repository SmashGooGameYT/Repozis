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

    public float RangePrepare = 2f;
    private int _Damage = 12;
     


    void Start()
    {
        HPnow = maxHP;
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

        rb.freezeRotation = false;

        //Уничтожение
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // part 2

    /*
    public void prepare()
    {
        if (PlayerLayer)
        {
            animEnemy.SetBool("PlayerInCombatZone", true);
        }
        else if (PlayerLayer)
        {
            animEnemy.SetBool("PlayerInCombatZone", false);
        }
    }
    */

    void Attack()
    {

        Collider2D[] HitPlayer = Physics2D.OverlapBoxAll(CombatZone.bounds.center, CombatZone.bounds.size, PlayerLayer);
        foreach (Collider2D Player in HitPlayer)
        {
            Player.GetComponent<HPScript>().TakeDamage(_Damage);
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(CombatZone.bounds.center, CombatZone.bounds.size);
    //}

}