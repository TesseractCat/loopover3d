using UnityEngine;
using UnityEngine.Serialization;

public class RotateCube : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("mouseSensitivity")]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private float _mouseSensitivity = 1f;

    [SerializeField]
    [FormerlySerializedAs("scrollWheelSensitivity")]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private float _scrollWheelSensitivity = 1f;

    private VoxelSpawner _voxelSpawner;
    private Vector3 _lastPos = Vector3.zero;

    private void Start()
    {
        _voxelSpawner = GetComponent<VoxelSpawner>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) _lastPos = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            var mouseOffset = Input.mousePosition - _lastPos;
            transform.RotateAround(
                Vector3.zero,
                Vector3.right,
                mouseOffset.y * _mouseSensitivity
            );
            transform.RotateAround(
                Vector3.zero,
                Vector3.up,
                -mouseOffset.x * _mouseSensitivity
            );
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            transform.RotateAround(
                Vector3.zero,
                Vector3.forward,
                Input.GetAxis("Mouse ScrollWheel") * _scrollWheelSensitivity
            );

        _lastPos = Input.mousePosition;

        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Vector3 vector = Vector3.zero;
                vector[(i + 2) % 3] = i < 3 ? 1 : -1;
                transform.forward = vector;
                transform.position = Vector3.zero;
                _voxelSpawner.CenterCube();
            }
        }
    }
}