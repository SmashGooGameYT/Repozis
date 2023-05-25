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


    [SerializeField] GameObject HPobject; // ������������ ���� UI ����������
    public Image HPbar; // ������ ��������
    public float maxHP; // ������������ ��������
    float HPnow; // ������� ��������

    private Vector2 moveV;
    public float speed = 4f;
    public float SlowSpeed = 1f;

    // ��
    [SerializeField] Transform Player;
    [SerializeField] Transform Home;
    public float AgrRange;
    public float fightRange;
    public float setSlowSpeedIfNearby = 3f;

    public float Stop = 0.5f;


    [SerializeField] private BoxCollider2D CombatZone; // ���� ���������� �����
    //[SerializeField] private BoxCollider2D DangerPlayer; // ���� �������������
    //��� �����
    [SerializeField] public LayerMask PlayerLayer;
    [SerializeField] Transform PlayerPos;


    public Transform DamageZone;
    public float RangePrepare = 0.5f;
    private float _damage = 0.12f;

    bool huntPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        c2 = GetComponent<Collider2D>();

        HPnow = maxHP;

        // � � ���������� ��������. �������� ������������ FaceRight �� ������� ������ �� �� ����������
        // 
        //
    }

    void Update()
    {
        HPbar.fillAmount = HPnow / maxHP;

        moveV.x = transform.position.x; // ������ ������


        float wherePlayer = Vector2.Distance(transform.position, Player.position); // ��������� �� ������
        float whereHome = Vector2.Distance(transform.position, Home.position); // ��������� �� ����

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
    }

    void GoPlayer()
    {
        if (Player.position.x < moveV.x)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (FaceRight)
            {
                transform.localScale *= new Vector2(-1, 1);
                FaceRight = false;
            }
        }
        else if (Player.position.x > moveV.x)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (!FaceRight)
            {
                transform.localScale *= new Vector2(-1, 1);
                FaceRight = true;
            }
        }
        huntPlayer = true;
        animEnemy.SetFloat("Running", Mathf.Abs(moveV.x));
    }

    void StopFight()
    {
        GoHome();
    }


    // ������������� // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

    public bool FaceRight = true;
    void GoHome()
    {
        if (Home.position.x < moveV.x)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (huntPlayer == true)
            {
                transform.localScale *= new Vector2(-1, 1);
                FaceRight = false;
                huntPlayer = false;
            }
        }

        else if (Home.position.x > moveV.x)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (huntPlayer == true)
            {
                transform.localScale *= new Vector2(-1, 1);
                FaceRight = true;
                huntPlayer= false;
            }
        }
        animEnemy.SetFloat("Running", Mathf.Abs(moveV.x));
    }

    public void HPreset()
    {
        HPnow = maxHP;
    }


    // ��������� �����  // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

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
        // ���������� ��������� � ������������ ���� �� ������������ ���
        c2.enabled = false;
        rb.isKinematic = true;

        //�������� ������
        animEnemy.SetBool("Die?", true);

        //�����������
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

    // �� ��� �������� � ������ �����. // // // // // // // // // // // // // // // // // // // // // // // // // // //



    // ���� ����� � ������� ������ �� �������� ������
    public void InAttackZone()
    {
        animEnemy.SetTrigger("Attack");
    }

    // ���� ����� � ���� ������ ��������
    public void InPlayerCombatZone()
    {
        animEnemy.SetBool("PlayerInCombatZone", true);
    }
    public void NoPlayerCombatZone()
    {
        animEnemy.SetBool("PlayerInCombatZone", false);
    }


    // ��������� �����

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