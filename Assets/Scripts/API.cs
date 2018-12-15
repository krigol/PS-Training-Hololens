using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        WWW request = new WWW(URL, rawFormData, headers);
        StartCoroutine(OnResponse(request));
    }

    private IEnumerator OnResponse(WWW req)
    {
        yield return req;
        
        var response = JsonUtility.FromJson<JsonPlaceholderResponseDto>(req.text);
        Debug.Log(response.Username + " " + response.id);
        
        var count = Convert.ToInt32(response.id)/10;

        var theWorld = GameObject.Find("The World");
        
        for (float i = 0; i < count; i++)
        {
            Instantiate(prefab, new Vector3(-4f + 0.6f * i, 0.5f, 3f), new Quaternion(), theWorld.transform);
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
