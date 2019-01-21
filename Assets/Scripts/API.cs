using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class API : MonoBehaviour {
    private const string URL = "https://jsonplaceholder.typicode.com/posts";
    private const string SfOauthUrl = "https://login.salesforce.com/services/oauth2/token";
    private const string SfOpportunityUrl = "/services/data/v43.0/sobjects/opportunity";
    private const string ApiKey = "";

    public GameObject prefab;
    public GameObject textMesh;
    
    // Use this for initialization
    void Start()
    {

    }

    public void GetSfOpportunities()
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "password");
        form.AddField("client_id", "");
        form.AddField("client_secret", "");
        form.AddField("username", "");
        form.AddField("password", "");

        UnityWebRequest req = UnityWebRequest.Post(SfOauthUrl, form);

        StartCoroutine(GetSfOauth(req));
    }

    IEnumerator GetSfOauth(UnityWebRequest req)
    {
        List<OpportunityDto> opportunityDtos = new List<OpportunityDto>();

        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log(req.error);
        }
        else
        {
            //Json parser didn't want to parse so I had to extract it in another way 
            var oauthToken = req.downloadHandler.text.Split('"')[3];
            
            UnityWebRequest opportunityRequest = UnityWebRequest.Get(SfOpportunityUrl);
            opportunityRequest.SetRequestHeader("Authorization", "Bearer " + oauthToken);

            yield return opportunityRequest.SendWebRequest();

            if (opportunityRequest.isNetworkError || opportunityRequest.isHttpError)
            {
                Debug.Log(opportunityRequest.error);
            }
            else
            {
                var recentItemsIndex = opportunityRequest.downloadHandler.text.IndexOf("recentItems");
                var recentItems = opportunityRequest.downloadHandler.text.Remove(0, recentItemsIndex);

                var recentItemObjectList = recentItems.Remove(0, 14);

                List<string> ids = new List<string>();

                while (recentItemObjectList.Contains("Id"))
                {
                    var idIndex = recentItemObjectList.IndexOf("Id");
                    recentItemObjectList = recentItemObjectList.Substring(idIndex);
                    recentItemObjectList = recentItemObjectList.Remove(0, 5);
                    ids.Add(recentItemObjectList.Substring(0,18));
                }

                foreach (var id in ids)
                {
                    UnityWebRequest opportunityDetailsRequest = UnityWebRequest.Get(SfOpportunityUrl + "/" + id);
                    opportunityDetailsRequest.SetRequestHeader("Authorization", "Bearer " + oauthToken);

                    yield return opportunityDetailsRequest.SendWebRequest();

                    if (opportunityDetailsRequest.isNetworkError || opportunityDetailsRequest.isHttpError)
                    {
                        Debug.Log(opportunityDetailsRequest.error);
                    }
                    else
                    {
                        var opportunityDto = new OpportunityDto();
                        var opportunityText = opportunityDetailsRequest.downloadHandler.text;

                        //Get Name
                        var nameIndex = opportunityText.IndexOf("Name");
                        //8 is symbols before actual value
                        opportunityText = opportunityText.Substring(nameIndex + 7);
                        var quoteIndex = opportunityText.IndexOf("\""); ;
                        opportunityDto.Name = opportunityText.Substring(0, quoteIndex);
                        
                        //Get Description
                        var descrIndex = opportunityText.IndexOf("Description");
                        //15 is symbols before actual value
                        opportunityText = opportunityText.Substring(descrIndex + 14);
                        quoteIndex = opportunityText.IndexOf("\""); ;
                        opportunityDto.Description = opportunityText.Substring(0, quoteIndex);
                        
                        //Get StageName
                        var stgNameIndex = opportunityText.IndexOf("StageName");
                        //13 is symbols before actual value
                        opportunityText = opportunityText.Substring(stgNameIndex + 12);
                        quoteIndex = opportunityText.IndexOf("\""); ;
                        opportunityDto.StageName = opportunityText.Substring(0, quoteIndex);
                        
                        //Get Amount
                        var amountIndex = opportunityText.IndexOf("Amount");
                        //13 is symbols before actual value
                        opportunityText = opportunityText.Substring(amountIndex + 8);
                        quoteIndex = opportunityText.IndexOf("\""); ;
                        // -2 to trim the ," since numbers don't have quotes
                        var amount = opportunityText.Substring(0, quoteIndex - 2);
                        opportunityDto.Amount = decimal.Parse(amount);

                        opportunityDtos.Add(opportunityDto);
                    }
                }
            }
        }
        var counter = 1;
        foreach (var opportunityDto in opportunityDtos)
        {
            var x = counter * 2f - 3;
            Debug.Log(counter);
            var gameObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            //-3 so we can fit the platform
            gameObj.transform.position = new Vector3(x, 1, 2.5f);
            gameObj.AddComponent<Rigidbody>();

            gameObj.GetComponent<Renderer>().material.color = getColorByStage(opportunityDto.StageName);
            //Instantiates the Object
            GameObject tempTextBox = Instantiate(textMesh, gameObj.transform);
            GameObject tempDescBox = Instantiate(textMesh, gameObj.transform);

            tempTextBox.transform.localPosition = new Vector3(0, 1, 0);
            tempDescBox.transform.localPosition = new Vector3(0, 0, 0.5f);

            //Grabs the TextMesh component from the game object
            TextMesh theText = tempTextBox.GetComponent<TextMesh>();
            theText.text = opportunityDto.Name;

            TextMesh theDescription = tempDescBox.GetComponent<TextMesh>();
            theDescription.text = opportunityDto.Description;

            for (int i = 1; i < opportunityDto.Amount/100; i++)
            {
                var ammountCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                ammountCube.AddComponent<Rigidbody>();
                ammountCube.GetComponent<Renderer>().material.color = getColorByStage(opportunityDto.StageName);

                ammountCube.transform.position = new Vector3(x, 1, 2.5f + i*1.5f);
            }

            //Sets the text.
            counter++;
        }
    }

    private Color getColorByStage(string stageName)
    {
        Color color;
        switch (stageName)
        {
            case "Prospecting":
                Debug.Log(11);
                color = new Color(1, 0.2f, 0);
                break;
            case "Qualification":
                Debug.Log(22);
                color = new Color(1, 0.4f, 0);
                break;
            case "Needs Analysis":
                Debug.Log(33);
                color = new Color(1, 0.6f, 0);
                break;
            case "Value Proposition":
                Debug.Log(44);
                color = new Color(1, 0.8f, 0);
                break;
            case "Id. Decision Makers":
                color = new Color(1, 1, 0);
                break;
            case "Perception Analysis":
                color = new Color(0.8f, 1, 0);
                break;
            case "Proposal/Price Quote":
                color = new Color(0.6f, 1, 0);
                break;
            case "Negotiation/Review":
                color = new Color(0.4f, 1, 0);
                break;
            case "Closed":
                color = new Color(0.2f, 1, 0);
                break;
            default:
                color = new Color(0, 0, 0);
                break;
        }

        return color;
    }

    public void GetCubes()
    {
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
        else
        {
            Debug.Log(req.downloadHandler.text);

            var response = JsonHelper.FromJson<JsonPlaceholderResponseDto>(req.downloadHandler.text);

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
                            Instantiate(prefab, new Vector3(-4f + 0.6f * length, height / 2, 3f + item.userId), new Quaternion(), theWorld.transform);

                            yield return new WaitForSeconds(0.05f);
                        }
                    }
                    currentId = item.userId;
                }
            }
        }
    }
}
