using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AlarmClock : MonoBehaviour
{
    public delegate void SetAlarmEventHandler();
    public static event SetAlarmEventHandler SetAlarmEvent;

    [SerializeField] private TMP_InputField hoursInput;
    [SerializeField] private TMP_InputField minutsInput;
    [SerializeField] private TMP_Text alarmHours;
    [SerializeField] private TMP_Text alarmMinuts;
    [SerializeField] private Alarm alarm;
    [SerializeField] private Timer timer;
    [SerializeField] private Image alarmTableView;

    private Color standart;

    private void Awake()
    {
        standart = alarmTableView.color;
    }

    private void OnEnable()
    {
        Alarm.AlarmEvent += SetTableUnactive;
    }

    private void OnDisable()
    {
        Alarm.AlarmEvent -= SetTableUnactive;
    }

    public void SetAlarmHours(string value)
    {
        SetTableUnactive();
        alarmHours.text = value;
    }

    public void SetAlarmMinuts(string value)
    {
        SetTableUnactive();
        alarmMinuts.text = value;
    }

    public void SetAlarm()
    {
        SetAlarmEvent?.Invoke();
        ClearTextFromInputFields();
        if(!String.IsNullOrEmpty(alarmHours.text) && !String.IsNullOrEmpty(alarmMinuts.text))
        {
            try
            {
                TimeSpan timeSpan = TimeSpan.Parse($"{alarmHours.text}:{alarmMinuts.text}");
                DateTime now = timer.GetNow();
                DateTime alarmTime = new DateTime(now.Year, now.Month, now.Day, timeSpan.Hours, timeSpan.Minutes, 0);
                alarm.Initialize(alarmTime, timer);
                SetTableActive();
            }
            catch (Exception)
            {
                SetTableUnactive();
                throw new Exception("Something wrong");
            }
        }
    }

    private void SetTableActive()
    {
        alarmTableView.color = Color.green;
    }

    private void SetTableUnactive()
    {
        alarmTableView.color = standart;
    }

    private void ClearTextFromInputFields()
    {
        hoursInput.text = "00";
        minutsInput.text = "00";
    }
}
