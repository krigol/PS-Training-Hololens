using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class API : MonoBehaviour {
    private const string URL = "https://jsonplaceholder.typicode.com/posts";
    private const string ApiKey = "RGAPI-91a7a2ed-a941-496e-a22f-74e4ddc8acd8";

    public GameObject prefab;
    
    // Use this for initialization
    void Start()
    {

    }

    public void GetCubes()
    {
        WWWForm form = new WWWForm();

        Dictionary<string, string> headers = form.headers;

        headers["X-Riot-Token"] = ApiKey;

        form.AddField("Username", "Gotta find a betah way");
        byte[] rawFormData = form.data;

        UnityWebRequest req = UnityWebRequest.Get(URL);

        StartCoroutine(OnResponse(req));
    }

    private IEnumerator OnResponse(UnityWebRequest req)
    {
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log(req.error);
        }

        Debug.Log(req.downloadHandler.text);

        var response = JsonHelper.FromJson<JsonPlaceholderResponseDto>(fixJson(req.downloadHandler.text));
        
        var theWorld = GameObject.Find("The World");

        var currentId = 0;

        foreach (var item in response)
        {
            if (currentId != item.userId)
            {

                for (int height = 1; height <= item.userId; height++)
                {
                    for (int length = 1; length <= item.userId; length++)
                    {
                        Instantiate(prefab, new Vector3(-4f + 0.6f * length, height/2, 3f + item.userId), new Quaternion(), theWorld.transform);

                        yield return new WaitForSeconds(0.05f);
                    }
                }
                currentId = item.userId;
            }
        }
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}
