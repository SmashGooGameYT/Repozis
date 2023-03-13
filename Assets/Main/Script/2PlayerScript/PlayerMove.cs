using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Компоненты
    public Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    // Функции для передвежения
    public Vector2 MoveV;
    public float speed;
    public float JumpForce;
    public bool FaceRight = true;

    // Переменные для прыжка
    public bool onGround = true;
    public LayerMask Ground;
    public bool onProps = true;
    public LayerMask Props;
    public Transform GroundCheck;
    public float checkRadius = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>(); // Присваеваем rb к компоненту RigidBody2D
        anim= GetComponent<Animator>(); // Присваеваем anim к компоненту Animator
        sr = GetComponent<SpriteRenderer>(); // Присваеваем sr к компоненту SpriteRender

    }

    // Объявление переменных для их обработки каждый кадр игры
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

    // Условие для передвежения
    void Walk()
    {
        MoveV.x = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("MoveX", Mathf.Abs(MoveV.x));
        rb.velocity = new Vector2(MoveV.x * speed, rb.velocity.y);
    }

    // Условие для поворота модельки персонажа
    void Flip()
    {
        if ((MoveV.x > 0 && !FaceRight) || (MoveV.x < 0 && FaceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            FaceRight = !FaceRight;
        } 
    }

    // Условие прыжка, если игрок стоит на земле
    void JumpGround()
    {
        if (Input.GetKeyDown(KeyCode.Space ) && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
    // Условие прыжка, если игрок стоит на предмете
    void JumpProps()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onProps)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }

    // Проверка на твердую поверхность под ногами
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