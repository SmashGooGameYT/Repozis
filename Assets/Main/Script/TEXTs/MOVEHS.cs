using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVEHS : MonoBehaviour
{
    public Animator animText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animText.SetBool("PlayerE", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animText.SetBool("PlayerE", false);
            Destroy(gameObject);
        }
    }
}
