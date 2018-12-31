using UnityEngine;

public class DisableTextOnInvisible : MonoBehaviour
{
    private VoxelSpawner _cube;

    private void Start()
    {
        _cube = FindObjectOfType<VoxelSpawner>();
    }

    private void Update()
    {
        var realPos = transform.parent.parent.localPosition -
                      new Vector3(
                          -(_cube.CubeSize / 2f) + 0.5f,
                          -(_cube.CubeSize / 2f) + 0.5f,
                          -(_cube.CubeSize / 2f) + 0.5f
                      );
        if (realPos.x * realPos.y * realPos.z == 0)
            GetComponent<MeshRenderer>().enabled = true;
        else
            GetComponent<MeshRenderer>().enabled = false;
    }
}