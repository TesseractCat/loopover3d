using UnityEngine;

public class OutlineToggle : MonoBehaviour {

    private bool _outlineEnabled = true;

    public void ToggleOutlines()
    {
        _outlineEnabled = !_outlineEnabled;
        if (_outlineEnabled)
        {
            FindObjectOfType<SetVoxelProperties>().GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", 0.02f);
        } else
        {
            FindObjectOfType<SetVoxelProperties>().GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", 0f);
        }
    }

}
