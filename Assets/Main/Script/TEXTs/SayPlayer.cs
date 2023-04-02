using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayPlayer : MonoBehaviour
{
    public Animator animText;
    public Animator ImageText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animText.SetBool("PlayerE", true);
            ImageText.SetBool("SayP", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animText.SetBool("PlayerE", false);
            ImageText.SetBool("SayP", false);
            Destroy(gameObject);
        }
    }
}
