using System.Collections.Generic;
using UnityEngine;

public class NumberToggle : MonoBehaviour
{
    public bool NumberEnabled = true;

    public void ToggleNumbers()
    {
        NumberEnabled = !NumberEnabled;
        if (NumberEnabled)
        {
            var voxels = FindObjectsOfType<SetVoxelProperties>();
            new List<SetVoxelProperties>(voxels).ForEach(
                v =>
                {
                    v.ShowNumber = true;
                    v.Redraw();
                }
            );
        }
        else
        {
            var voxels = FindObjectsOfType<SetVoxelProperties>();
            new List<SetVoxelProperties>(voxels).ForEach(
                v =>
                {
                    v.ShowNumber = false;
                    v.Redraw();
                }
            );
        }
    }
}