using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public Action<Arrow> OnClick;
    public bool IsActive { get; set; }
    [SerializeField] protected Image myImage;

    [SerializeField] protected Color standart;
    [SerializeField] protected Color hover;

    private void OnEnable()
    {
        AlarmClock.SetAlarmEvent += SetUnactive;
    }

    private void OnDisable()
    {
        AlarmClock.SetAlarmEvent -= SetUnactive;
    }

    public void Rotate(float angleRotate)
    {
        myImage.rectTransform.eulerAngles = new Vector3(0f, 0f, angleRotate);
    }

    public void SetActive()
    {
        myImage.color = new Color(hover.r, hover.g, hover.b, myImage.color.a);
    }

    public void SetUnactive()
    {
        myImage.color = new Color(standart.r, standart.g, standart.b, myImage.color.a);
    }
}
