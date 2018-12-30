using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnHover : MonoBehaviour {

    Color startColor;
    public Color toColor;
    Color lerpToColor;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
        startColor = GetComponent<MeshRenderer>().material.GetColor("_Color");
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                lerpToColor = toColor;
            }
            else
            {
                lerpToColor = startColor;
            }
        } else
        {
            lerpToColor = startColor;
        }

        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.Lerp(GetComponent<MeshRenderer>().material.color, lerpToColor, Time.deltaTime * 15));
    }
}
