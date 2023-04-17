using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour


{
    public float force = 200;

    private Rigidbody2D rb;

    public PlayerMove isRotated;


    public Transform DamageZone;
    public LayerMask EnemyLayer;
    public float Range = 0.3f;
    public float RA = 0.05f;

    public GameObject This;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isRotated = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();


        if (!isRotated.FaceRight)
        {
            rb.AddForce(new Vector2(-force, 0));
        }
        else if (isRotated.FaceRight == true)
        {
            rb.AddForce(new Vector2(force, 0));
        }

        Destroy(gameObject, 1f);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(DamageZone.position, Range, EnemyLayer);
            foreach (Collider2D enemy in HitEnemies)
            {
                enemy.GetComponent<AIbanditL>().TakeDamage(RA);
            }
        }
        if (other.tag == "Ground")
        {
            Destroy(This);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(DamageZone.position, Range);
    }

}
