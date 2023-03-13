using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HPScript : MonoBehaviour
{

    private Animator anim;

    [SerializeField] private int HPplayer = 100;
    int PlayerHPnow;

    private void Start()
    {
        PlayerHPnow = HPplayer;
    }


    public void TakeDamage(int damage)
    {
        HPplayer -= damage;

        // anim.SetTrigger("Hurt"); (СДЕЛАТЬ)

        if (HPplayer < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // анимация смерти (СДЕЛАТЬ)


        // возрат в главное меню
        SceneManager.LoadScene(0);
    }
}
