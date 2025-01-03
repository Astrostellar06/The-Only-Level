using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI level;
    [SerializeField] TMPro.TextMeshProUGUI deaths;
    [SerializeField] TMPro.TextMeshProUGUI time;
    private float ms = 0;
    private float seconds = 0;
    private float minutes = 0;
    private void Update()
    {
        ms += Time.deltaTime;
        if (ms >= 1)
        {
            seconds++;
            ms--;
            if (seconds == 60)
            {
                seconds = 0;
                minutes++;
            }
        }
        string minute = minutes < 10 ? "0" + minutes : minutes.ToString();
        string second = seconds < 10 ? "0" + seconds : seconds.ToString();
        string mil = ms*1000 < 100 && ms*1000 >= 10 ? "0" + Math.Round(ms * 1000) : ms*1000 < 10 ? "00" + Math.Round(ms * 1000) : Math.Round(ms * 1000).ToString();
        level.text = "Level " + Manager.instance.level.value.ToString() + ":";
        deaths.text = (Manager.instance.deaths > 1 ? "Deaths:  " : "Death: ") + Manager.instance.deaths.ToString();
        time.text = minute + ":" + second + ":" + mil;
    }
}
