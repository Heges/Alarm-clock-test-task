using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;

public class TimeService : MonoBehaviour
{
    public static TimeService instance;
    public bool TimeIsReady => timeIsReady;

    private bool timeIsReady;
    private string url = "https://worldtimeapi.org/api/ip";
    private DateTime currentData;
    private IEnumerator timeCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void StartTask()
    {
        timeIsReady = false;
        if (timeCoroutine == null)
        {
            timeCoroutine = GetTime(SerializeResult);
            StartCoroutine(timeCoroutine);
        }
        else
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
            timeCoroutine = GetTime(SerializeResult);
            StartCoroutine(timeCoroutine);
        }
    }

    public DateTime GetDateTime()
    {
        return new DateTime(currentData.Year,
            currentData.Month,
            currentData.Day,
            currentData.Hour,
            currentData.Minute,
            currentData.Second,
            currentData.Kind);
    }

    private void SerializeResult(string str)
    {
        foreach (var c in str.Split(','))
        {
            if (c.StartsWith(@"""datetime"""))
            {
                foreach (var splitedC in c.Split('"'))
                {
                    if (!String.IsNullOrEmpty(splitedC) && !Char.IsNumber(splitedC[0]))
                    {
                        continue;
                    }
                    else
                    {
                        try
                        {
                            currentData = DateTime.Parse(splitedC);
                            
                        }
                        catch
                        {
                            //ничего не делаем по идее если строка в апи соответствует стандарту ISO 8601 то все гуд
                        }
                        
                    }
                        
                }
                timeIsReady = true;
                return;
            }
        }
        
    }

    private IEnumerator GetTime(Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode !=
          (long)System.Net.HttpStatusCode.OK)
            {
                Debug.LogError("response error: " + request.responseCode);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }
}
