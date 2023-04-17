using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HPScript : MonoBehaviour
{

    public Animator anim;

    public Image HPbar;
    public int maxHP = 1;
    public int HP;


    private Collider2D c2;
    private Rigidbody2D rb;

    private void Start()
    {
        HPbar = GetComponent<Image>();
        HP = maxHP;

        rb = GetComponent<Rigidbody2D>();
        c2 = GetComponent<Collider2D>();
    }

    void Update()
    {
        HPbar.fillAmount = HP / maxHP;
    }


    public void TakeDamage(int damage)
    {
        HP -= damage;
        anim.SetTrigger("Herted");

        if (HP < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // �������� ������ (�������)
        anim.SetBool("HP0", true);
        c2.enabled = false;
       

        // ���������� (�������)

        // ������ � ������� ����
        SceneManager.LoadScene(0);
    }
}
