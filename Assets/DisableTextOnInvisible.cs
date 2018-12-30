using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTextOnInvisible : MonoBehaviour {

    VoxelSpawner cube;

    void Start()
    {
        cube = FindObjectOfType<VoxelSpawner>();
    }

    void Update () {
        
        Vector3 realPos = transform.parent.parent.localPosition - new Vector3(-(cube.cubeSize / 2f) + 0.5f, -(cube.cubeSize / 2f) + 0.5f, -(cube.cubeSize / 2f) + 0.5f);
        if (realPos.x * realPos.y * realPos.z == 0)
        {
            GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
	}
}
