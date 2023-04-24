using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAttackZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            GameObject.FindWithTag("Enemy").GetComponent<AIbanditL>().InAttackZone();
        }
    }


    void OnTriggerStay2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            GameObject.FindWithTag("Enemy").GetComponent<AIbanditL>().InAttackZone();
        }
    }
}
