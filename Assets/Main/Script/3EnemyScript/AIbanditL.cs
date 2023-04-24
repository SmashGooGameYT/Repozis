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

    private Vector2 MoveV;
    public bool FaceRight = true;
    public float speed = 4f;
    float SlowSpeed = 1;


    [SerializeField] Transform Player;
    [SerializeField] Transform Home;
    public float AgrRange;
    public float fightRange;


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


        // Как оставливать противника если совсем рядом игрок И он рядом с точкой дома
        // B GoHome проблемка 
        // И с разваратом проблема. Пробовал адаптировать FaceRight из скрипта игрока но не получилось, пока стоит полурабочий local.scale 
        // 
        //
        // Всё работает в целом, противник правда не бъёт, но причина я так понимаю заключается в том что у него вечно состояние бега
    }

    void Update()
    {
        HPbar.fillAmount = HPnow / maxHP;

        MoveV.x = transform.position.x; // Замена строки

        
        float wherePlayer = Vector2.Distance(transform.position, Player.position);
        if (wherePlayer < AgrRange)
        {
             GoPlayer();
        }

        else
        {
            //StopFight();
        }
    }

    void GoPlayer()
    {
        if (Player.position.x < MoveV.x)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

        else if (Player.position.x > MoveV.x)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        animEnemy.SetFloat("Running", Mathf.Abs(MoveV.x));
    }

    void StopFight()
    {
        rb.velocity = Vector2.zero;
        GoHome();
    }


    // Востоновление // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    void GoHome()
    {
        if (MoveV.x > Home.position.x)
        {
            rb.velocity = new Vector2(-speed, 0);
            transform.localScale *= new Vector2(1, 1);
        }
        else if (MoveV.x < Home.position.x)
        {
            rb.velocity = new Vector2(speed, 0);
            transform.localScale *= new Vector2(-1, 1);
        }
        animEnemy.SetFloat("Running", Mathf.Abs(MoveV.x));

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
        }
    }

    void Die()
    {
        c2.enabled = false;
        rb.isKinematic = true;

        //Анимация смерти
        animEnemy.SetBool("Die?", true);

        //Уничтожение
        Destroy(gameObject, 3);
        Destroy(HPobject, 1);
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
        rb.velocity = new Vector2(SlowSpeed, rb.velocity.y);
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