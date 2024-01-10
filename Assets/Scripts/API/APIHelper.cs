using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class APIHelper : MonoBehaviour
{
    
    public static APIHelper Instance;

    public FactsPanelUI FactsPanelUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public IEnumerator RequestFacts(string _URL, Action<List<Fact>> callback)
    {
        yield return StartCoroutine(GetRequest(_URL, callback));
    }

    private IEnumerator GetRequest<T>(string url, Action<T> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                T result = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                callback?.Invoke(result);
            }
        }
    }
}
