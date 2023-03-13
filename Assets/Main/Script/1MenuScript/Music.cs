using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource MainTheme;



    void Start()
    {
        MainTheme.Play();
    }
}
