using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ForgeEnter : MonoBehaviour
{
    public Animator CaveWarning;


    private void OnTriggerEnter2D(Collider2D  PlayerCol)
    {
        if (PlayerCol.tag == "Player")
        {
            CaveWarning.SetBool("PlayerEnter", true);
        }
    }
    private void OnTriggerStay2D(Collider2D PlayerCol)
    {
        if (PlayerCol.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(4);
            }
        }


    }
    private void OnTriggerExit2D(Collider2D PlayerCol)
    {
        if (PlayerCol.tag == "Player")
        {
            CaveWarning.SetBool("PlayerEnter", false);
        }
    }
}
