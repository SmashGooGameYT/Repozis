using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InCombatZone : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindWithTag("Enemy").GetComponent<AIbanditL>().InPlayerCombatZone();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindWithTag("Enemy").GetComponent<AIbanditL>().NoPlayerCombatZone();
        }
    }
}
