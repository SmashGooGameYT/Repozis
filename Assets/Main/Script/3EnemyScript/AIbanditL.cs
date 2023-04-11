using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AIbanditL : MonoBehaviour
{
    private Collider2D c2;
    public Animator animEnemy;
    private Rigidbody2D rb;

    public int maxHP = 100;
    int HPnow;

    public bool FaceRight = true;
    public Vector2 MoveV;
    public float speed = 5f;

    [SerializeField] Transform Player;
    [SerializeField] float AgrRange;


    // part 2

    [SerializeField] private BoxCollider2D CombatZone;
    [SerializeField] public LayerMask PlayerLayer;
    [SerializeField] Transform PlayerPos;

    public Transform DamageZone;
    public float RangePrepare = 0.5f;
    private int _damage = 12;

    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        c2 = GetComponent<Collider2D>();
        HPnow = maxHP;
    }

    void Update()
    {
        walk();
        Flip();

    }

    void walk()
    {
        rb.velocity = new Vector2(MoveV.x * speed, rb.velocity.y);
        animEnemy.SetFloat("Running", Mathf.Abs(MoveV.x));
    }


    void Flip()
    {
        if ((MoveV.x < 0 && !FaceRight) || (MoveV.x > 0 && FaceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            FaceRight = !FaceRight;
        }
    }




    // Получение урона  // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

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
        c2.enabled= false;
        rb.isKinematic = true;

        //Анимация смерти
        animEnemy.SetBool("Die?", true);

        //Уничтожение
        Destroy(gameObject, 5);
    }

    // Всё что связанно с атакой снизу. // // // // // // // // // // // // // // // // // // // // // // // // // // //

    public void InPlayerCombatZone()
    {
        animEnemy.SetBool("PlayerInCombatZone", true);
        rb.velocity = Vector2.zero;
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


    // Gizmo // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    void OnDrawGizmosSelected()
    {
        if (DamageZone == null)
            return;

        Gizmos.DrawWireSphere(DamageZone.position, RangePrepare);
    }
}