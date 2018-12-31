using UnityEngine;
using UnityEngine.Serialization;

public class RotateCube : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("mouseSensitivity")]
    // ReSharper disable once ConvertToConstant.Local
    private readonly float _mouseSensitivity = 1f;

    [SerializeField] [FormerlySerializedAs("scrollWheelSensitivity")]
    // ReSharper disable once ConvertToConstant.Local
    private readonly float _scrollWheelSensitivity = 1f;

    private Vector3 _lastPos = Vector3.zero;

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
    }
}