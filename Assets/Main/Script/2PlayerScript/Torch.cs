using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject torch;
    bool presed = false;

    void Start()
    {
        torch.SetActive(false);
    }

    void Update()
    {
        if (presed == false)
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                torch.SetActive(true);
                presed = true;
            }
        }

        else if (presed == true)
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
            { 
                torch.SetActive(false);
                presed = false;
            }
        }
    }
}