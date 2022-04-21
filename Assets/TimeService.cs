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
    private string url1 = "http://worldclockapi.com/api/json/est/now";
    private string url2 = "http://worldclockapi.com/api/json/utc/now";
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
            timeCoroutine = GetTime(new string[] { url, url1, url2 }, SerializeResult);
            StartCoroutine(timeCoroutine);
        }
        else
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
            timeCoroutine = GetTime(new string[] { url, url1, url2 }, SerializeResult);
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

    private void SerializeResult(string[] str)
    {
        List<DateTime> dateTime = new List<DateTime>();
        int index = str.Length;
        int indexer = 0;
        while (indexer < index)
        {
            string currentStr = str[indexer];
            indexer++;

            foreach (var c in currentStr.Split(','))
            {
                if (c.StartsWith(@"""datetime""") || c.StartsWith(@"""currentDateTime"""))
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
                                DateTime tm = DateTime.Parse(splitedC);
                                dateTime.Add(tm);
                            }
                            catch
                            {
                                //ничего не делаем по идее если строка в апи соответствует стандарту ISO 8601 то все гуд
                            }
                            
                        }

                    }
                    break;
                }
        }
        
        }
        currentData = dateTime[0];
        timeIsReady = true;

    }

    private IEnumerator GetTime(string[] str, Action<string[]> callback)
    {
        string[] results = new string[str.Length];
        int index = str.Length;
        int indexer = 0;
        while (indexer < index)
        {
            string currentStr = str[indexer];
            
            using (UnityWebRequest request = UnityWebRequest.Get(currentStr))
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
                    results[indexer] = request.downloadHandler.text;
                }
                indexer++;
            }
        }

        callback(results);
    }
}
