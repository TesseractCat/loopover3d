using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutOnClick : MonoBehaviour {

    public VoxelMover cube;
    public bool forward = true;

    void Start()
    {
        cube = FindObjectOfType<VoxelMover>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID() && Input.GetMouseButtonDown(0))
            {
                if (forward)
                {
                    GameObject[] nodules = GameObject.FindGameObjectsWithTag("Nodule");
                    for (int n = 0; n < nodules.Length; n++)
                    {
                        if (nodules[n].GetInstanceID() != transform.parent.parent.gameObject.GetInstanceID())
                        {
                            nodules[n].SetActive(false);
                        }
                    }

                    cube.CutForward(cube.transform.InverseTransformDirection(transform.forward));
                } else
                {
                    cube.CutBackward(cube.transform.InverseTransformDirection(-transform.forward), true);
                }
            }
        }
    }
}
