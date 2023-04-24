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

    private Vector2 MoveV;
    public bool FaceRight = true;
    public float speed = 4f;
    float SlowSpeed = 1;


    [SerializeField] Transform Player;
    [SerializeField] Transform Home;
    public float AgrRange;
    public float fightRange;


    [SerializeField] private BoxCollider2D CombatZone; // ���� ���������� �����
    //[SerializeField] private BoxCollider2D DangerPlayer; // ���� �������������
    //��� �����
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


        // ��� ����������� ���������� ���� ������ ����� ����� � �� ����� � ������ ����
        // B GoHome ��������� 
        // � � ���������� ��������. �������� ������������ FaceRight �� ������� ������ �� �� ����������, ���� ����� ����������� local.scale 
        // 
        //
        // �� �������� � �����, ��������� ������ �� ����, �� ������� � ��� ������� ����������� � ��� ��� � ���� ����� ��������� ����
    }

    void Update()
    {
        HPbar.fillAmount = HPnow / maxHP;

        MoveV.x = transform.position.x; // ������ ������

        
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


    // ������������� // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

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

        // ��� ���� ������� �� ��� ���� ��������� ������ �� ����� �� ���������� ��
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
        }
    }

    void Die()
    {
        c2.enabled = false;
        rb.isKinematic = true;

        //�������� ������
        animEnemy.SetBool("Die?", true);

        //�����������
        Destroy(gameObject, 3);
        Destroy(HPobject, 1);
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
        rb.velocity = new Vector2(SlowSpeed, rb.velocity.y);
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