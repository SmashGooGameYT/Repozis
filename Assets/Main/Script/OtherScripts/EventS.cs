using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventS : MonoBehaviour
{
    public GameObject G1;
    public GameObject G2;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(G1);
            Destroy(G2, 1);
        }
    }
}
