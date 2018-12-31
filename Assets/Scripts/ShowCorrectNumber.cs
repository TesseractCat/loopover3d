using UnityEngine;

public class ShowCorrectNumber : MonoBehaviour {
    private VoxelSpawner _cube;
    public int CorrectNumber;
    private bool _showingNumber = false;

    void Start()
    {
        _cube = FindObjectOfType<VoxelSpawner>();
    }

    void Update () {
		if ((Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButton(2)) || (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.E)) && !_showingNumber)
        {
            _showingNumber = true;
            GetComponent<SetVoxelProperties>().DisplayNumber(CorrectNumber);
        }
        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(2))
        {
            _showingNumber = false;
            GetComponent<SetVoxelProperties>().Redraw();
        }
	}
}
