using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine;
using UnityEngine.UI;

public class AIbanditL : MonoBehaviour
{
    public Animator animEnemy;
    private Collider2D c2;
    private Rigidbody2D rb;

    [SerializeField] Image HPbar;
    private float maxHP = 0.5f;
    float HPnow;

    public bool FaceRight = true;
    public Vector2 MoveV;
    public float speed = 5f;
    float SlowSpeed = 1;

    [SerializeField] Transform Player;
    [SerializeField] float AgrRange;

    // part 2

    [SerializeField] private BoxCollider2D CombatZone; // Зона подготовки атаки
    [SerializeField] private BoxCollider2D DangerPlayer; // Зона приследования
    [SerializeField] public LayerMask PlayerLayer;
    [SerializeField] Transform PlayerPos;

    public Transform DamageZone;
    public float RangePrepare = 0.5f;
    private int _damage = 12;

    // Патруль
    public Transform[] spots;
    public float startWait;
    private float wait;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        c2 = GetComponent<Collider2D>();

        HPbar = GetComponent<Image>();
        HPnow = maxHP;



    }

    void Update()
    {
        walk();
        SlowW();
        Flip();



        HPbar.fillAmount = HPnow / maxHP;


    }

    public void walk()
    {
        if (DangerPlayer.tag != "Player") 
        {
            rb.velocity = new Vector2(MoveV.x * speed, rb.velocity.y);
            animEnemy.SetFloat("Running", Mathf.Abs(MoveV.x));

        }
    }

    public void SlowW()
    {
        if (CombatZone.tag == "Player")
        {
            rb.velocity = new Vector2(MoveV.x * SlowSpeed, rb.velocity.y);
            animEnemy.SetBool("PlayerInCombatZone", true);

        }
    }


    void Flip()
    {
        if ((MoveV.x < 0 && !FaceRight) || (MoveV.x > 0 && FaceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            FaceRight = !FaceRight;
        }
    }



    // Востоновление // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    public void GoHome()
    {

    }




    public void HP()
    {
        HPnow = HPnow;
    }

    public void HPreset()
    {
        HPnow = maxHP;
    }


    // Получение урона  // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    public void TakeDamage(float damage)
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
        c2.enabled = false;
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


    // Gizmos // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    void OnDrawGizmosSelected()
    {
        if (DamageZone == null)
            return;

        Gizmos.DrawWireSphere(DamageZone.position, RangePrepare);
    }
}