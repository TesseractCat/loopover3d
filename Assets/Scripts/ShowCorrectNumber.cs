using UnityEngine;

public class ShowCorrectNumber : MonoBehaviour {

    VoxelSpawner cube;
    public int correctNumber;
    bool showingNumber = false;

    void Start()
    {
        cube = FindObjectOfType<VoxelSpawner>();
    }

    void Update () {
		if ((Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButton(2)) || (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.E)) && !showingNumber)
        {
            showingNumber = true;
            GetComponent<SetVoxelProperties>().DisplayNumber(correctNumber);
        }
        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(2))
        {
            showingNumber = false;
            GetComponent<SetVoxelProperties>().Redraw();
        }
	}
}
