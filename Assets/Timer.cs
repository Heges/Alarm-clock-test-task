using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public delegate void SecondsChangeEventHandler(int number);
    public delegate void MinutsChangeEventHandler(int minuts);
    public delegate void HoursChangeEventHandler(int hours);

    public static event SecondsChangeEventHandler SecondsChangeEvent;
    public static event MinutsChangeEventHandler MinutsChangeEvent;
    public static event HoursChangeEventHandler HoursChangeEvent;

    [SerializeField] private  TMP_Text hoursText;
    [SerializeField] private  TMP_Text minutsText;
    [SerializeField] private  TMP_Text secondsText;

    private int seconds;
    private int minuts;
    private int hours;
    private DateTime currentData;

    private bool synhronized = false;
    private IEnumerator synhronizer;

    private void Awake()
    {
        synhronizer = SyncrhonizeTime();
    }

    private void OnEnable()
    {
        synhronized = false;
    }

    private IEnumerator SyncrhonizeTime()
    {
        TimeService.instance.StartTask();
        while (!TimeService.instance.TimeIsReady)
        {
            yield return null;
        }
        currentData = TimeService.instance.GetDateTime();

        hours = currentData.Hour;
        minuts = currentData.Minute;
        seconds = currentData.Second;

        SetHoursText(hours);
        SetMinutsText(minuts);
        SetSeconds(seconds);

        synhronized = true;
        synhronizer = null;
    }

    private float timeToUpdate = 0f;
    private void Update()
    {
        if (synhronized)
        {
            timeToUpdate += Time.deltaTime;

            if(timeToUpdate >= 1f)
            {
                seconds = ((int)timeToUpdate + seconds) % 60;
                timeToUpdate = 0;

                SetSeconds(seconds);

                if (seconds % 60 == 0)
                {
                    minuts = (minuts + 1) % 60;

                    SetMinutsText(minuts);

                    if (minuts % 60 == 0)
                    {
                        hours = (hours + 1) % 24;
                        SetHoursText(hours);
                    }
                }
            }
        }
        else
        {
            if (synhronizer == null)
            {
                synhronizer = SyncrhonizeTime();
                StartCoroutine(synhronizer);
            }
            else
            {
                StopCoroutine(synhronizer);
                StartCoroutine(synhronizer);
            }
        }
        
    }

    private void SetHoursText(int hours)
    {
        //hoursText.text = $"{hours:D2}";
        hoursText.text = hours.ToString("00");
        HoursChangeEvent?.Invoke(hours);
    }

    private void SetMinutsText(int min)
    {
        //minutsText.text = $"{min:D2}";
        minutsText.text = min.ToString("00");
        MinutsChangeEvent?.Invoke(minuts);
    }

    private void SetSeconds(int seconds)
    {
        //secondsText.text = $"{ seconds:D2}";
        secondsText.text = seconds.ToString("00");
        SecondsChangeEvent?.Invoke(seconds);
    }

    public DateTime GetNow()
    {
        return new DateTime(currentData.Year, currentData.Month, currentData.Day, hours, minuts, seconds);
    }
}
