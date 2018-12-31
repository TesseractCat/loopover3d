using UnityEngine;
using UnityEngine.Serialization;

public class CutOnClick : MonoBehaviour {
    private VoxelMover _cube;
    [SerializeField, FormerlySerializedAs("forward")]
    private bool _forward = true;

    void Start()
    {
        _cube = FindObjectOfType<VoxelMover>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID() && Input.GetMouseButtonDown(0))
            {
                if (_forward)
                {
                    GameObject[] nodules = GameObject.FindGameObjectsWithTag("Nodule");
                    for (int n = 0; n < nodules.Length; n++)
                    {
                        if (nodules[n].GetInstanceID() != transform.parent.parent.gameObject.GetInstanceID())
                        {
                            nodules[n].SetActive(false);
                        }
                    }

                    _cube.CutForward(_cube.transform.InverseTransformDirection(transform.forward));
                } else
                {
                    _cube.CutBackward(_cube.transform.InverseTransformDirection(-transform.forward), true);
                }
            }
        }
    }
}
