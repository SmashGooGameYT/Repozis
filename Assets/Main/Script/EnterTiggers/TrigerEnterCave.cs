using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TrigerEnterCave : MonoBehaviour
{
    public Animator CaveWarning;
    [SerializeField] public LayerMask Player;


    private void OnTriggerEnter2D(Collider2D  PlayerCol)
    {
            CaveWarning.SetBool("PlayerEnter", true);
    }
    private void OnTriggerStay2D(Collider2D PlayerCol)
    {

            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(2);
            }

    }
    private void OnTriggerExit2D(Collider2D Player)
    {

        CaveWarning.SetBool("PlayerEnter", false);

    }

}
