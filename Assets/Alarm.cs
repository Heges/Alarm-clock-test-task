using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Alarm : MonoBehaviour
{
    public delegate void AlarmEventHandler();
    public static event AlarmEventHandler AlarmEvent;
    public bool IsSetUp => isSetUp;

    private Timer timer;
    private DateTime alarm;
    private bool isSetUp;

    public void Initialize(DateTime a, Timer t)
    {
        alarm = a;
        timer = t;
        isSetUp = true;
    }

    private void Update()
    {
        if (isSetUp)
        {
            var date = timer.GetNow();
            if (date.Hour >= alarm.Hour && date.Minute >= alarm.Minute)
            {
                Debug.Log("ALARM ALARM ALARM!!!!!!");
                isSetUp = false;
                AlarmEvent?.Invoke();
            }
        }
    }
}
