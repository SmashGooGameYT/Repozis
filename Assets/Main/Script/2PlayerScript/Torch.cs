using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject torch;
    public bool Presed = false;

    void Start()
    {
        torch.SetActive(false);
    }

    void Update()
    {
        if (Presed == false)
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                torch.SetActive(true);
                Presed= true;
            }
        }
        else if ( Presed == true)
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
            { 
                torch.SetActive(false);
                Presed= false;
            }
        }
    }
}