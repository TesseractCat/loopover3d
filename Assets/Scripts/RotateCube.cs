using UnityEngine;
using UnityEngine.Serialization;

public class RotateCube : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("mouseSensitivity")]
    private readonly float _mouseSensitivity = 1f;

    [SerializeField] [FormerlySerializedAs("scrollWheelSensitivity")]
    private readonly float _scrollWheelSensitivity = 1f;

    private Vector3 lastPos = Vector3.zero;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) lastPos = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            var mouseOffset = Input.mousePosition - lastPos;
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

        lastPos = Input.mousePosition;
    }
}