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


    [SerializeField] GameObject HPobject; // Унижчтожение всех UI противника
    public Image HPbar; // Полоса здоровья
    public float maxHP; // Максимальное здоровье
    float HPnow; // текущие здоровье

    private Vector2 moveV;
    public bool FaceRight = true;
    public float speed = 4f;
    public float SlowSpeed = 1f;

    // ИИ
    [SerializeField] Transform Player;
    [SerializeField] Transform Home;
    public float AgrRange;
    public float fightRange;
    public float setSlowSpeedIfNearby = 3f;

    public float Stop = 0.5f;


    [SerializeField] private BoxCollider2D CombatZone; // Зона подготовки атаки
    //[SerializeField] private BoxCollider2D DangerPlayer; // Зона приследования
    //Для урона
    [SerializeField] public LayerMask PlayerLayer;
    [SerializeField] Transform PlayerPos;


    public Transform DamageZone;
    public float RangePrepare = 0.5f;
    private float _damage = 0.12f;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        c2 = GetComponent<Collider2D>();

        HPnow = maxHP;

        // И с разваратом проблема. Пробовал адаптировать FaceRight из скрипта игрока но не получилось
        // 
        //
    }

    void Update()
    {
        HPbar.fillAmount = HPnow / maxHP;

        moveV.x = transform.position.x; // Замена строки


        float wherePlayer = Vector2.Distance(transform.position, Player.position); // дистанция до игрока
        float whereHome = Vector2.Distance(transform.position, Home.position); // дистанция до дома

        if (wherePlayer < AgrRange)
        {
            GoPlayer();
            if (wherePlayer < setSlowSpeedIfNearby)
            {
                rb.velocity = new Vector2(SlowSpeed, rb.velocity.y);
                if (wherePlayer < fightRange)
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }
        }

        else
        {
            StopFight();
            if (whereHome < Stop)
            {
                rb.velocity = new Vector2(0, 0);
                animEnemy.SetFloat("Running", 0);
                HPreset();
            }
        }

        StopMove();
        Flip();
    }

    void GoPlayer()
    {
        if (Player.position.x < moveV.x)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (Player.position.x > moveV.x)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        animEnemy.SetFloat("Running", Mathf.Abs(moveV.x));
    }

    void StopFight()
    {
        GoHome();
    }


    // проблема в координатах, как решить хз
    void Flip()
    {
        if ((moveV.x > 0 && !FaceRight) || (moveV.x < 0 && FaceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            FaceRight = !FaceRight;
        }
    }


    // Востоновление // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    void GoHome()
    {
        if (Home.position.x < moveV.x)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

        else if (Home.position.x > moveV.x)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        animEnemy.SetFloat("Running", Mathf.Abs(moveV.x));

        // Как сюда вписать то что если противник пришёл на точку он востановит ХП
    }

    public void HPreset()
    {
        HPnow = maxHP;
    }


    // Получение урона  // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    public void TakeDamage(float damage)
    {
        HPnow -= damage;

        animEnemy.SetTrigger("Hurt");

        if (HPnow <= 0)
        {
            Die();
            rb.velocity = new Vector2(0, 0);
        }
    }

    void Die()
    {
        // отключение колладера и переключение тела на кинетический тип
        c2.enabled = false;
        rb.isKinematic = true;

        //Анимация смерти
        animEnemy.SetBool("Die?", true);

        //Уничтожение
        Destroy(gameObject, 3);
        Destroy(HPobject, 1);
    }

    void StopMove()
    {
        if (HPnow <= 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    // Всё что связанно с атакой снизу. // // // // // // // // // // // // // // // // // // // // // // // // // // //



    // Если игрок в облости аттаки то проводит аттаку
    public void InAttackZone()
    {
        animEnemy.SetTrigger("Attack");
    }

    // Если игрок в зоне боевых действий
    public void InPlayerCombatZone()
    {
        animEnemy.SetBool("PlayerInCombatZone", true);
    }
    public void NoPlayerCombatZone()
    {
        animEnemy.SetBool("PlayerInCombatZone", false);
    }


    // Нанесение урона

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