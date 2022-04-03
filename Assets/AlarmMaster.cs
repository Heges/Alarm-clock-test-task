using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class AlarmMaster : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image myImage;
    [SerializeField] private MinutArrow minutArrow;
    [SerializeField] private HourArrow hourArrow;
    [SerializeField] private AlarmClock alarmClock;

    private Arrow currentArrow;

    private float angleRotate;

    private void OnEnable()
    {
        minutArrow.OnClick += ChoiseArrow;
        hourArrow.OnClick += ChoiseArrow;
    }

    private void OnDisable()
    {
        minutArrow.OnClick -= ChoiseArrow;
        hourArrow.OnClick -= ChoiseArrow;
    }

    private void ChoiseArrow(Arrow ar)
    {
        if(currentArrow != null)
        {
            currentArrow.SetUnactive();
            currentArrow = ar;
            currentArrow.SetActive();
        }
        else
        {
            currentArrow = ar;
            currentArrow.SetActive();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentArrow)
        {
            Vector3 newPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 myPos = mainCamera.ScreenToWorldPoint(transform.position);
            Vector3 dir = newPos - myPos;
            angleRotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            currentArrow.Rotate(angleRotate);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var decimalValue = 3f - (1f / 30f) * (angleRotate + 90f % 360);
        if (decimalValue < 0)
            decimalValue += 12.0f;

        int hours = (int)decimalValue % 12;

        if (currentArrow && currentArrow.CompareTag("AlarmMinut"))
        {
            int minutes = Mathf.RoundToInt((decimalValue * 5) % 60);
            alarmClock.SetAlarmMinuts($"{minutes:D2}");
            
        }
        else if (currentArrow && currentArrow.CompareTag("AlarmHour"))
        {
            hours = Mathf.FloorToInt(decimalValue % 12);
            alarmClock.SetAlarmHours($"{hours:D2}");
        }
    }
}
