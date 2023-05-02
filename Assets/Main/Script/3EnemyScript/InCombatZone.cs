using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InCombatZone : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            GameObject.FindWithTag("Enemy").GetComponent<AIbanditL>().InPlayerCombatZone();
        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            GameObject.FindWithTag("Enemy").GetComponent<AIbanditL>().InPlayerCombatZone();
        }
    }


    void OnTriggerExit2D(Collider2D c)
    {
        if (c.tag != "Player")
        {
            GameObject.FindWithTag("Enemy").GetComponent<AIbanditL>().NoPlayerCombatZone();
        }
    }
}
