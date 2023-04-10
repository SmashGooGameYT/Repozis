using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventResume : MonoBehaviour
{
    public GameObject This;
    public GameObject G1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(This);
            G1.SetActive(true);
        }
    }
}
