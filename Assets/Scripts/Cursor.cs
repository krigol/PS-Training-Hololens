using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
    private MeshRenderer meshRenderer;

    public GameObject FocusedObject { get; set; }

    // Use this for initialization
    void Start () {
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //Do a raycast into the world based on the user's head position and orientation
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            //If we hit a hologram display cursor
            meshRenderer.enabled = true;

            //Move cursor to the hit
            this.transform.position = hitInfo.point;


            //Rotate to hug surface
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

            var newFocusedObject = hitInfo.collider.gameObject;

            if (FocusedObject != null && newFocusedObject != FocusedObject)
            {
                FocusedObject.SendMessage("OnReset");
            }

            FocusedObject = newFocusedObject;
            FocusedObject.SendMessage("OnSelect");
        }
        else
        {
            //If nothing hit disable cursor
            meshRenderer.enabled = false;

            if (FocusedObject != null)
            {
                FocusedObject.SendMessage("OnReset");
            }

            FocusedObject = null;
        }
	}
}
