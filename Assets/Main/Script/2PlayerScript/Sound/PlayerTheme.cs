using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTheme : MonoBehaviour
{
    [SerializeField] private AudioSource PlayerWithMusic;
    void Start()
    {
        PlayerWithMusic.Play();
    }
}
