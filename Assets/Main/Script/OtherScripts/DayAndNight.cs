using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class DayAndNight : MonoBehaviour
{
    public Volume volume; // Здесь сам объект "Шум"

    public float tick; 
    public float seconds;
    public float mins;
    public float hours;

    public bool activateLights; // ВКЛ / ВЫКЛ свет
    public GameObject[] lights; // Массив источников света которые нужно будет включать/выключать


    void Start()
    {
        volume = gameObject.GetComponent<Volume>();
    }

    void FixedUpdate() // ФикседАпдейт потому что более точно будет считать время
    {
        CalcTime();
    }

    public void CalcTime() // Создание системы времени
    {
        seconds += Time.fixedDeltaTime * tick; 

        if (seconds >= 60) // 60 секунд = 1 минута
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) // 60 минута = 1 час
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) // если больше 24 то обнуляется часы.
        {
            hours = 0;
        }
        ControlVolume(); 
    }

    public void ControlVolume() // Метод который отвечает за переключения дня и ночи
    {
        if (hours >= 18 && hours < 21) // темнеет в 17:00 и до 21:00
        {
            volume.weight = mins / 60;
            if (activateLights == false) // Включение света если выключен
            {
                if (mins > 45) // немного ждём для включения
                {
                    for (int i = 0; i < lights.Length; i++) // подсчёт массива
                    {
                        lights[i].SetActive(true); // Включение света
                    }
                    activateLights = true; // после включения делаем булевую пометку что свет включен
                }
            }
        }

        if (hours >= 7 && hours < 9) // Рассвет в 5:00 - и до 9:00
        {
            volume.weight = 0.9875f - mins / 60;
            if (activateLights == true) // выключение света если выключен
            {
                if (mins > 45) // немного ждём для выключения
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false); // Выключение света
                    }
                    activateLights = false; // после включения делаем булевую пометку что свет выключен
                }
            }
        }
    }
}
