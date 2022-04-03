using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] private Image secundsArrow;
    [SerializeField] private Image minutsArrow;
    [SerializeField] private Image hoursArrow;

    private void OnEnable()
    {
        Timer.HoursChangeEvent += ChangedHours;
        Timer.SecondsChangeEvent += ChangedSeconds;
        Timer.MinutsChangeEvent += ChangeMinuts;
    }

    private void OnDisable()
    {
        Timer.HoursChangeEvent -= ChangedHours;
        Timer.SecondsChangeEvent -= ChangedSeconds;
        Timer.MinutsChangeEvent -= ChangeMinuts;
    }

    private void ChangedSeconds(int number)
    {
        float t = number / 60f;
        ChangePositionSecundsArrow(t);
    }

    private void ChangedHours(int hours)
    {
        float t = hours / 12f;
        ChangePositionHoursArrow(t);
    }

    private void ChangeMinuts(int minuts)
    {
        float t = minuts / 60f;
        ChangePositionMinutsArrow(t);
    }

    private void ChangePositionSecundsArrow(float secunds)
    {
        secundsArrow.rectTransform.eulerAngles = new Vector3(0f, 0f, -secunds * Mathf.PI * 2f) * Mathf.Rad2Deg;
    }

    private void ChangePositionMinutsArrow(float minuts)
    {
        minutsArrow.rectTransform.eulerAngles = new Vector3(0f, 0f, -minuts * Mathf.PI * 2f) * Mathf.Rad2Deg;
    }

    private void ChangePositionHoursArrow(float hours)
    {
        hoursArrow.rectTransform.eulerAngles = new Vector3(0f, 0f, -hours * Mathf.PI * 2f) * Mathf.Rad2Deg;
    }

    public void SetAlphaHourArrow(float value)
    {
        hoursArrow.color = new Color(hoursArrow.color.r, hoursArrow.color.g, hoursArrow.color.g, value);
    }
    
    public void SetAlphaMinutArrow(float value)
    {
        minutsArrow.color = new Color(minutsArrow.color.r, minutsArrow.color.g, minutsArrow.color.g, value);
    }
}
