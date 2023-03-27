using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ����������
    public Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    // ���������� ��� ������������
    public Vector2 MoveV;
    public float speed;
    public float JumpForce;
    public bool FaceRight = true;
    public float CrounchSpeed = 1;
    public float ForceSlide = 100;

    // ���������� ��� ������
    public bool onGround = true;
    public LayerMask Ground;
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
        PlyerSits();
        PlayerSliding();
        CheckGround();
        Flip();

        IgnoreEnemyLayer();

        Checkingladder();
        LaddersMech();
        ladderUpDown();
        LADDERS();
        CurrectLadder();
    }

    // ������� ��� ������������
    void Walk()
    {
        MoveV.x = Input.GetAxisRaw("Horizontal");
        if  (!onLadders)
        {
            rb.velocity = new Vector2(MoveV.x * speed, rb.velocity.y);
        }
        else if (onLadders)
        {
            rb.velocity = Vector2.zero;
        }
        anim.SetFloat("MoveX", Mathf.Abs(MoveV.x));
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(6, 10, true);
            Invoke("IgnorePlatformOFF", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Space ) && onGround && !onLadders)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }

    // ����� ��� ����������� � ��������� ���� ���������� ������������ �����
    void IgnorePlatformOFF()
    {
        Physics2D.IgnoreLayerCollision(6, 10, false);
    }
    // ������������� ����� ��������
    void IgnoreEnemyLayer()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        Physics2D.IgnoreLayerCollision(11, 11, true);
    }


    // �������� �� ������� ����������� ��� ������
    void CheckGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGroundes", onGround);
    }

    void PlyerSits()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            rb.velocity = new Vector2(MoveV.x * CrounchSpeed, rb.velocity.y);
            anim.SetBool("Sits", true);
        }
        if (Input.GetKey(KeyCode.LeftControl)) 
        {
            rb.velocity = new Vector2(MoveV.x * CrounchSpeed, rb.velocity.y);
            anim.SetBool("Sits", true);
        }

        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            rb.velocity = new Vector2(MoveV.x * CrounchSpeed, rb.velocity.y);
            anim.SetBool("Sits", false);
        }
    }

    void PlayerSliding()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("Sliding", true);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Sliding", false);
        }
    }


    // ��������

    public Transform Check_ladder;
    public float ladder_Radius = 0.1f;
    public bool checkedladder;
    public LayerMask ladderlayer;
    public float ladderSpeed = 2f;

    public Transform botton_ladder;
    public bool bottoncheckedladder;

    private void OnDrawGizmos() // ��������� ���� ����� ������� �������� �� ������, ���� �� �������� � �� ����������
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Check_ladder.position, ladder_Radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(botton_ladder.position, ladder_Radius);
    }

    void Checkingladder()
    {
        checkedladder = Physics2D.OverlapPoint(Check_ladder.position, ladderlayer);
        bottoncheckedladder = Physics2D.OverlapPoint(botton_ladder.position, ladderlayer);
    }
    void LaddersMech()
    {
        if (onLadders) 
        { 
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(rb.velocity.x, MoveV.y * ladderSpeed);
        }
        else 
        { 
            rb.bodyType = RigidbodyType2D.Dynamic; 
        }
    }

    void ladderUpDown()
    {
        MoveV.y = Input.GetAxisRaw("Vertical");
        anim.SetFloat("MoveY", MoveV.y);
    }

    public bool onLadders;
    void LADDERS()
    {
        if (checkedladder || bottoncheckedladder) 
        {
            if (!checkedladder && bottoncheckedladder)// ������ ��������
            {
                if (Input.GetAxisRaw("Vertical") > 0)      { onLadders = false; }
                else if (Input.GetAxisRaw("Vertical") < 0) { onLadders = true; }
            }
            else if (checkedladder && bottoncheckedladder)// �� ��������
            {
                if (Input.GetAxisRaw("Vertical") > 0)      { onLadders = true; }
                else if (Input.GetAxisRaw("Vertical") < 0) { onLadders = true; }
                else if  (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0) { onLadders = false; }
            }
            else if (checkedladder && !bottoncheckedladder)// ��� ���������
            {
                if (Input.GetAxisRaw("Vertical") > 0)      { onLadders = true; }
                else if (Input.GetAxisRaw("Vertical") < 0) { onLadders = false; }
            }
        }
        else { onLadders = false; }

        LaddersMech();

        anim.SetBool("onLadder", onLadders);
    }


    bool corrected = true;
    void CurrectLadder()
    {
        if (onLadders &&  corrected) { corrected = !corrected; rb.velocity = Vector2.zero; LedderCenter(); }
        else if (!onLadders && !corrected) { corrected = true; }
    }

    float ledderCenter;
    void LedderCenter()
    {
        if (checkedladder) { ledderCenter = Physics2D.OverlapPoint(Check_ladder.position, ladderlayer).gameObject.transform.position.x; }
        else if (bottoncheckedladder) { ledderCenter = Physics2D.OverlapPoint(botton_ladder.position, ladderlayer).GetComponent<BoxCollider2D>().bounds.center.x; }
        transform.position = new Vector2 (ledderCenter, transform.position.y);
    }
}

//  if (checkedladder) { ledderCenter = Physics2D.OverlapPoint(Check_ladder.position, ladderlayer).GetComponent<BoxCollider2D>().bounds.center.x; }     void LedderCenter() 2-3 
