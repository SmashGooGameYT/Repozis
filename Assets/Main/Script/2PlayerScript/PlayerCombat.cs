using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animCombat;
    private float LightDamage = 0.12f;
    private float RunDamage = 0.18f;
    private float HeavyAttack = 0.20f;
    private float time = 0;

    public Transform DamageZone;
    public LayerMask EnemyLayer;
    public float Range = 1f;
    public Rigidbody2D rb;

    public GameObject Slash;
    public Transform startPos;
    public float RangeAttack = 0.05f;


    void Start()
    {
        animCombat = GetComponent<Animator>();
    }

    void Update()
    {
        LightAttack();
        RunAttack();
        HeavyAtack();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            time++;
        }
    }


    void LightAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animCombat.SetBool("Attack", true);
        }

        else if (Input.GetKey(KeyCode.Mouse0))
        {
            animCombat.SetBool("Attack", false);
        }
    }

    void RunAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animCombat.SetTrigger("RunAttack");
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            animCombat.SetBool("RunAttack", false);
        }
    }

    void HeavyAtack()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && time > 0.5)
        {
            animCombat.SetTrigger("HeavyA");
        }
        else 
        {
            animCombat.SetBool("HeavyA", false);
        }
    }


    // Нанесения легкого урона противнику
    void LHit()
    {
        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(DamageZone.position, Range, EnemyLayer);
        foreach(Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<AIbanditL>().TakeDamage(LightDamage);
        }

    }
    // Нанесения урона с разбегу противнику
    void RHit()
    {
        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(DamageZone.position, Range, EnemyLayer);
        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<AIbanditL>().TakeDamage(RunDamage);
        }
    }
    void HHit()
    {
        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(DamageZone.position, Range, EnemyLayer);
        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<AIbanditL>().TakeDamage(HeavyAttack);
        }
    }

    public void RangeHit()
    {
        //Instantiate(Slash, startPos.position, Quaternion.identity);
        float Rotate = 0;
        bool Looking = GetComponent<PlayerMove>().FaceRight;
        if (Looking)
        {
            Rotate = 0;
        }
        else
        {
            Rotate = 180;
        }

        Instantiate(Slash, startPos.position, Quaternion.identity);

        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(DamageZone.position, Range, EnemyLayer);
        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<AIbanditL>().TakeDamage(RangeAttack);
        }
    }

    // Отрисовка зоны атаки
    void OnDrawGizmosSelected()
    {
        if (DamageZone == null)
            return;

        Gizmos.DrawWireSphere(DamageZone.position, Range);
    }
}

