using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLogic : MonoBehaviour {

    private Color originalColor;
    private Vector3 originalVector; 

    // Use this for initialization
    void Start()
    {
        var newColor = new Color(Random.Range((float)0.0, (float)1.0), Random.Range((float)0.0, (float)1.0), Random.Range((float)0.0, (float)1.0));
        this.GetComponent<Renderer>().material.color = newColor;

        originalColor = newColor;
        originalVector = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < -1.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnSelect()
    {
        this.GetComponent<Renderer>().material.color = Color.magenta;
    }

    void OnReset()
    {
        this.GetComponent<Renderer>().material.color = originalColor;
    }

    void OnResetBlock()
    {
        this.transform.position = originalVector;
    }
}
