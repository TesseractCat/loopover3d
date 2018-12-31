using UnityEngine;
using UnityEngine.Serialization;

public class CutOnClick : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("forward")]
    // ReSharper disable once ConvertToConstant.Local
    private readonly bool _forward = true;

    private VoxelMover _cube;

    private void Start()
    {
        _cube = FindObjectOfType<VoxelMover>();
    }

    private void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
            if (hit.transform.gameObject.GetInstanceID() ==
                gameObject.GetInstanceID() &&
                Input.GetMouseButtonDown(0))
            {
                if (_forward)
                {
                    var nodules =
                        GameObject.FindGameObjectsWithTag("Nodule");
                    for (var n = 0; n < nodules.Length; n++)
                        if (nodules[n].GetInstanceID() !=
                            transform.parent.parent.gameObject.GetInstanceID())
                            nodules[n].SetActive(false);

                    _cube.CutForward(
                        _cube.transform.InverseTransformDirection(
                            transform.forward
                        )
                    );
                }
                else
                {
                    _cube.CutBackward(
                        _cube.transform.InverseTransformDirection(
                            -transform.forward
                        ),
                        true
                    );
                }
            }
    }
}