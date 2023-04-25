using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class DayAndNight : MonoBehaviour
{
    public Volume volume; // ����� ��� ������ "���"

    public float tick; 
    public float seconds;
    public float mins;
    public float hours;

    public bool activateLights; // ��� / ���� ����
    public GameObject[] lights; // ������ ���������� ����� ������� ����� ����� ��������/���������


    void Start()
    {
        volume = gameObject.GetComponent<Volume>();
    }

    void FixedUpdate() // ������������ ������ ��� ����� ����� ����� ������� �����
    {
        CalcTime();
    }

    public void CalcTime() // �������� ������� �������
    {
        seconds += Time.fixedDeltaTime * tick; 

        if (seconds >= 60) // 60 ������ = 1 ������
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) // 60 ������ = 1 ���
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) // ���� ������ 24 �� ���������� ����.
        {
            hours = 0;
        }
        ControlVolume(); 
    }

    public void ControlVolume() // ����� ������� �������� �� ������������ ��� � ����
    {
        if (hours >= 18 && hours < 21) // ������� � 17:00 � �� 21:00
        {
            volume.weight = mins / 60;
            if (activateLights == false) // ��������� ����� ���� ��������
            {
                if (mins > 45) // ������� ��� ��� ���������
                {
                    for (int i = 0; i < lights.Length; i++) // ������� �������
                    {
                        lights[i].SetActive(true); // ��������� �����
                    }
                    activateLights = true; // ����� ��������� ������ ������� ������� ��� ���� �������
                }
            }
        }

        if (hours >= 7 && hours < 9) // ������� � 5:00 - � �� 9:00
        {
            volume.weight = 0.9875f - mins / 60;
            if (activateLights == true) // ���������� ����� ���� ��������
            {
                if (mins > 45) // ������� ��� ��� ����������
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false); // ���������� �����
                    }
                    activateLights = false; // ����� ��������� ������ ������� ������� ��� ���� ��������
                }
            }
        }
    }
}
