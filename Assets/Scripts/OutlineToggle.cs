using UnityEngine;

public class OutlineToggle : MonoBehaviour
{
    private bool _outlineEnabled = true;

    public void ToggleOutlines()
    {
        _outlineEnabled = !_outlineEnabled;
        FindObjectOfType<SetVoxelProperties>()
            .GetComponent<Renderer>()
            .sharedMaterial.SetFloat(
                "_FirstOutlineWidth",
                _outlineEnabled ? 0.02f : 0f
            );
    }
}