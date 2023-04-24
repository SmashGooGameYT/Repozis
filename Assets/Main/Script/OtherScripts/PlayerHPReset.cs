using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPReset : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D c)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (c.tag == "Player")
            {
                GameObject.FindWithTag("Player").GetComponent<HPScript>().healYourSelf();
            }
        }
    }
}
