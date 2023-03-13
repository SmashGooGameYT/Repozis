using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ����������
    public Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    // ������� ��� ������������
    public Vector2 MoveV;
    public float speed;
    public float JumpForce;
    public bool FaceRight = true;

    // ���������� ��� ������
    public bool onGround = true;
    public LayerMask Ground;
    public bool onProps = true;
    public LayerMask Props;
    public Transform GroundCheck;
    public float checkRadius = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>(); // ����������� rb � ���������� RigidBody2D
        anim= GetComponent<Animator>(); // ����������� anim � ���������� Animator
        sr = GetComponent<SpriteRenderer>(); // ����������� sr � ���������� SpriteRender

    }

    // ���������� ���������� ��� �� ��������� ������ ���� ����
    void Update()
    {
        Walk();
        JumpGround();
        JumpProps();
        PlyerSits();
        PlayerSliding();
        CheckGround();
        Flip();
    }

    // ������� ��� ������������
    void Walk()
    {
        MoveV.x = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("MoveX", Mathf.Abs(MoveV.x));
        rb.velocity = new Vector2(MoveV.x * speed, rb.velocity.y);
    }

    // ������� ��� �������� �������� ���������
    void Flip()
    {
        if ((MoveV.x > 0 && !FaceRight) || (MoveV.x < 0 && FaceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            FaceRight = !FaceRight;
        } 
    }

    // ������� ������, ���� ����� ����� �� �����
    void JumpGround()
    {
        if (Input.GetKeyDown(KeyCode.Space ) && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
    // ������� ������, ���� ����� ����� �� ��������
    void JumpProps()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onProps)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }

    // �������� �� ������� ����������� ��� ������
    void CheckGround()
    {
        onProps = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Props);
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGroundes", onGround || onProps);
    }

    void PlyerSits()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Sits", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("Sits", false);
        }
    }

    void PlayerSliding()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            rb.velocity = new Vector2(MoveV.x * speed, rb.velocity.y);
            anim.SetBool("Sliding", true);
        }
        else
        {
            anim.SetBool("Sliding", false);
        }
    }
}