using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BarEnter : MonoBehaviour
{
    public Animator BarWarning;

    private void OnTriggerEnter2D(Collider2D  PlayerCol)
    {
        if (PlayerCol.tag == "Player")
        {
            BarWarning.SetBool("PlayerEnter", true);
        }
    }
    private void OnTriggerStay2D(Collider2D PlayerCol)
    {
        if (PlayerCol.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(3);
            }
        }


    }
    private void OnTriggerExit2D(Collider2D PlayerCol)
    {
        if (PlayerCol.tag == "Player")
        {
            BarWarning.SetBool("PlayerEnter", false);
        }
    }
}
