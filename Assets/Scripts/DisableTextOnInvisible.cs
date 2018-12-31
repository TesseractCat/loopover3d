using UnityEngine;

public class DisableTextOnInvisible : MonoBehaviour {
    private VoxelSpawner _cube;

    void Start()
    {
        _cube = FindObjectOfType<VoxelSpawner>();
    }

    void Update () {
        
        Vector3 realPos = transform.parent.parent.localPosition - new Vector3(-(_cube.cubeSize / 2f) + 0.5f, -(_cube.cubeSize / 2f) + 0.5f, -(_cube.cubeSize / 2f) + 0.5f);
        if (realPos.x * realPos.y * realPos.z == 0)
        {
            GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
	}
}
