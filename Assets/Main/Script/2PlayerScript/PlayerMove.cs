using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Компоненты
    public Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    // Переменные для передвижения
    private Vector2 MoveV;
    public float speed;
    public float JumpForce;
    public bool FaceRight = true;


    // Переменные для приседа
    public float CrounchSpeed = 2f;
    public float ForceSlide = 100;
    public Transform TopCheck;
    public float TopCheckRadius;
    public LayerMask Roof;
    private bool JumpLock = false;

    // Переменные для прыжка
    public bool onGround = true;
    public LayerMask Ground;
    public Transform GroundCheck;
    public float checkRadius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Присваеваем rb к компоненту RigidBody2D
        anim = GetComponent<Animator>(); // Присваеваем anim к компоненту Animator
        sr = GetComponent<SpriteRenderer>(); // Присваеваем sr к компоненту SpriteRender

        TopCheckRadius = TopCheck.GetComponent<CircleCollider2D>().radius; // Присваеваем радиус объекта TopCheck к значению радиуса

        WallcheckRadiusDown = WallCheckDown.GetComponent<CircleCollider2D>().radius;
        gravityDef = rb.gravityScale;

    }

    // Объявление переменных для их обработки каждый кадр игры
    void Update()
    {
        Walk();
        JumpGround();
        PlyerSits();
        PlayerSliding();
        Flip();

        IgnoreEnemyLayer();

        Checkingladder();
        LaddersMech();
        ladderUpDown();
        LADDERS();
        CurrectLadder();

        MoveOnWall();
        WallJump();
        ledgeGO();
    }

    private void FixedUpdate()
    {
        ChikingWall();
        CheckingLedge();
        CheckGround();
    }

    // Условие для передвежения
    void Walk()
    {
        if (!BlockMoveXY) 
        {
            MoveV.x = Input.GetAxisRaw("Horizontal");
            if (!onLadders)
            {
                rb.velocity = new Vector2(MoveV.x * speed, rb.velocity.y);
            }
            else if (onLadders)
            {
                rb.velocity = Vector2.zero;
            }
            anim.SetFloat("MoveX", Mathf.Abs(MoveV.x));
        }
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(6, 10, true);
            Invoke("IgnorePlatformOFF", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Space ) && onGround && !onLadders && !JumpLock)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }

    // Метод для спрыгивания с платформы путём отключения столкновения слоев
    void IgnorePlatformOFF()
    {
        Physics2D.IgnoreLayerCollision(6, 10, false);
    }

    // Игнорирование слоев противка
    void IgnoreEnemyLayer()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        Physics2D.IgnoreLayerCollision(11, 11, true);
    }


    // Проверка на твердую поверхность под ногами
    void CheckGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGroundes", onGround);
    }

    void PlyerSits()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            JumpLock = true;
            rb.velocity = new Vector2(MoveV.x * CrounchSpeed, rb.velocity.y);
            anim.SetBool("Sits", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && Physics2D.OverlapCircle(TopCheck.position, TopCheckRadius, Roof))
        {
            JumpLock = true;
            rb.velocity = new Vector2(MoveV.x * CrounchSpeed, rb.velocity.y);
            anim.SetBool("Sits", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            JumpLock = false;
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

    // Ползанье по стенам
    public bool onWall = true;
    public bool onWallUp;
    public bool onWallDown;
    public LayerMask Wall;
    public Transform WallCheckUp;
    public Transform WallCheckDown;
    public float WallcheckRayDistance = 1f;
    public float WallcheckRadiusDown;
    private float UpDownSpeed = 0.5f;
    private float SlideWallSpeed = 1.5f;
    private float gravityDef;
    private bool BlockMoveXY;
    private bool BlockMoveXYledge;
    public float JumpWallTime = 0.5f;
    private float TimerJumpWall;
    public Vector2 JumpAngle = new Vector2(3.5f, 3);
    public bool onLedge;
    public float ledgeRay = 0.5f;
    public float offsetY;
    private float minCor = 0.01f;
    public Transform FinPosC;

    void ChikingWall()
    {
        onWallUp = Physics2D.Raycast(WallCheckUp.position, new Vector2(transform.localScale.x, 0), WallcheckRayDistance, Wall);
        onWallDown = Physics2D.OverlapCircle(WallCheckDown.position, WallcheckRadiusDown, Wall);
        onWall = onWallUp && onWallDown;
        anim.SetBool("OnWall", onWall);
    }

    void CheckingLedge()
    {
        if (onWallUp)
        {
            onLedge = !Physics2D.Raycast
            (
            new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRay),
            new Vector2(transform.localScale.x, 0),
            WallcheckRayDistance,
            Wall
            );
        }
        else { onLedge = false; }

        anim.SetBool("onLedge", onLedge);

        if (onLedge && Input.GetAxisRaw("Vertical") != -1)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0,0);
            correctLedge();
        }
    }

    void correctLedge()
    {
        offsetY = Physics2D.Raycast
        (
        new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y + ledgeRay),
        Vector2.down,
        ledgeRay,
        Ground
        ).
        distance;

        if (offsetY > minCor * 1.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - offsetY + minCor, transform.position.z);
        }
    }

    void ledgeGO()
    {
        BlockMoveXYledge = true;
        if (onLedge && Input.GetKeyDown(KeyCode.Space))
        {
            finishledge();
            anim.Play("OnWallPlayerClimb");
        }
    }

    void finishledge()
    {
        transform.position = new Vector3(FinPosC.position.x, FinPosC.position.y, FinPosC.position.z);
        BlockMoveXYledge = false;
    }


    void MoveOnWall()
    {
        if (onWall && !onGround)
        {
            MoveV.y = Input.GetAxisRaw("Vertical");
            anim.SetFloat("UpDown", MoveV.y);

            if  (!BlockMoveXY && MoveV.y == 0) 
            {
                    rb.gravityScale = 0;
                    rb.velocity = new Vector2(0, SlideWallSpeed);
            }

            if (!BlockMoveXYledge)
            {
                if (MoveV.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, MoveV.y * UpDownSpeed / 2);
                }
                else if (MoveV.y < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, MoveV.y * UpDownSpeed);
                }
            }
        }
        else if (!onGround && !onWall) 
        {
            rb.gravityScale = gravityDef;
        }
    }

    void WallJump()
    {
        if (onWall && !onGround && Input.GetKeyDown(KeyCode.Space))
        {
            BlockMoveXY = true;

            MoveV.x = 0.5f;

            anim.StopPlayback();

            transform.localScale *= new Vector2(-1, 1);
            FaceRight = !FaceRight;

            rb.gravityScale = gravityDef;
            rb.velocity = new Vector2(0, 0);

            rb.velocity = new Vector2(transform.localScale.x * JumpAngle.x, JumpAngle.y);
        }
        if (BlockMoveXY && (TimerJumpWall += Time.deltaTime) >= JumpWallTime)
        {
            if (onWall || onGround || Input.GetAxisRaw("Horizontal") != 0)
            {
                BlockMoveXY = false;
                TimerJumpWall = 0;
            }
        }
    }



    // Лестница

    public Transform Check_ladder;
    public float ladder_Radius = 0.1f;
    public bool checkedladder;
    public LayerMask ladderlayer;
    public float ladderSpeed = 2f;

    public Transform botton_ladder;
    public bool bottoncheckedladder;

    private void OnDrawGizmos() // Отрисовка двух точек которые отвечают за слежку, есть ли лестница в их коллайдере
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Check_ladder.position, ladder_Radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(botton_ladder.position, ladder_Radius);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(WallCheckUp.position, new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y));

        Gizmos.color = Color.red;
        Gizmos.DrawLine 
            (
            new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRay),
            new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y + ledgeRay)
            );

        Gizmos.color = Color.green;
        Gizmos.DrawLine
            (
                new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y + ledgeRay),
                new Vector2(WallCheckUp.position.x + WallcheckRayDistance * transform.localScale.x, WallCheckUp.position.y)
            );
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
            if (!checkedladder && bottoncheckedladder)// СВЕРХУ ЛЕСТНИЦЫ
            {
                if (Input.GetAxisRaw("Vertical") > 0)      { onLadders = false; }
                else if (Input.GetAxisRaw("Vertical") < 0) { onLadders = true; }
            }
            else if (checkedladder && bottoncheckedladder)// НА ЛЕСТНИЦЕ
            {
                if (Input.GetAxisRaw("Vertical") > 0)      { onLadders = true; }
                else if (Input.GetAxisRaw("Vertical") < 0) { onLadders = true; }
                else if  (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0) { onLadders = false; }
            }
            else if (checkedladder && !bottoncheckedladder)// ПОД ЛЕСТНИЦЕЙ
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
