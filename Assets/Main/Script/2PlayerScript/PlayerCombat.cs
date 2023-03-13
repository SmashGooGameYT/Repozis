using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animCombat;
    private int LightDamage = 6;
    private int RunDamage = 13;

    public Transform DamageZone;
    public LayerMask EnemyLayer;
    public float Range = 1f;



    // Start is called before the first frame update
    void Start()
    {
        animCombat = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LightAttack();
        RunAttack();
    }

    void LightAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animCombat.SetTrigger("Attack");
            LHit();
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
            //RHit();
        }

        else if (Input.GetKey(KeyCode.Mouse0))
        {
            animCombat.SetBool("RunAttack", false);
        }
    }

    // ��������� ������� ����� ����������
    void LHit()
    {
        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(DamageZone.position, Range, EnemyLayer);
        foreach(Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<AIbanditL>().TakeDamage(LightDamage);
        }
    }
    // ��������� ����� � ������� ����������
    void RHit()
    {
        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(DamageZone.position, Range, EnemyLayer);
        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<AIbanditL>().TakeDamage(RunDamage);
        }
    }

    // ��������� ���� �����
    void OnDrawGizmosSelected()
    {
        if (DamageZone == null)
            return;

        Gizmos.DrawWireSphere(DamageZone.position, Range);
    }
}
