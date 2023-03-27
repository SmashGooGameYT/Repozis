using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    Image HPbar;
    public float maxHP = 100f;
    public float HP;

    void Start()
    {
        HPbar = GetComponent<Image>();
        HP = maxHP;
    }

    void Update()
    {
        HPbar.fillAmount = HP / maxHP;
    }
}
