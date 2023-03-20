using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCaveText : MonoBehaviour
{
    public Animator WarningText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WarningText.SetBool("PlayerIn", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        WarningText.SetBool("PlayerIn", false);

    }

}
