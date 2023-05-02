using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsOnMenu : MonoBehaviour
{
    public Animator animFade;
    private float time = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            time++;
        }
    }


    public void START()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && time > 1)
        {
            animFade.SetBool("Hide?", true);
            SceneManager.LoadScene(1);
        }
    }

    public void EXIT()
    {
        Application.Quit();
    }
}