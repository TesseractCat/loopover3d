using UnityEngine;

public class ShowCorrectNumber : MonoBehaviour
{
    private bool _showingNumber;
    public int CorrectNumber;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButton(2) ||
            Input.GetMouseButtonDown(2) &&
            !Input.GetKey(KeyCode.E) &&
            !_showingNumber)
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