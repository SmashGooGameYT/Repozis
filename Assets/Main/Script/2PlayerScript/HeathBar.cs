using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    public Image HPbar;
    public int maxHP = 1;
    public int HP;

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
