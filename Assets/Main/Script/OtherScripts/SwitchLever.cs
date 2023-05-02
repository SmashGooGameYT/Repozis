using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLever : MonoBehaviour
{
    [SerializeField] Transform player;
    float nearby = 1.5f;
    bool presed = false;
    Animator animLever;
    public Animator animDoors;

    void Start()
    {
        animLever = GetComponent<Animator>();
    }

    void Update()
    {
        float wherePlayer = Vector2.Distance(transform.position, player.position);

        if (presed == false)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (wherePlayer <= nearby)
                {
                    animLever.SetBool("ON / OFF", true);
                    animDoors.SetBool("Open / Close", true);
                    presed = true;
                }
            }
        }

        else if (presed == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (wherePlayer <= nearby)
                {
                    animLever.SetBool("ON / OFF", false);
                    animDoors.SetBool("Open / Close", false);
                    presed = false;
                }
            }
        }
    }
}
