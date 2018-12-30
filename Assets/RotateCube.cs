using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour {
    
	void Start () {
		
	}

    public float mouseSensitivity = 1f;
    public float scrollWheelSensitivity = 1f;

    Vector3 lastPos = Vector3.zero;

	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            lastPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseOffset = Input.mousePosition - lastPos;
            transform.RotateAround(Vector3.zero, Vector3.right, mouseOffset.y * mouseSensitivity);
            transform.RotateAround(Vector3.zero, Vector3.up, -mouseOffset.x * mouseSensitivity);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, Input.GetAxis("Mouse ScrollWheel") * scrollWheelSensitivity);
        }
        lastPos = Input.mousePosition;
	}
}
