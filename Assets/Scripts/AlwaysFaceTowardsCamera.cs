using UnityEngine;

[ExecuteInEditMode]
public class AlwaysFaceTowardsCamera : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}