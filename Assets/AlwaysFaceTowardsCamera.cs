using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlwaysFaceTowardsCamera : MonoBehaviour {
    
    
	void Update () {
        transform.LookAt(Camera.main.transform, Vector3.up);
	}
}
