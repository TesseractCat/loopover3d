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
            SetVoxelProperties[] voxels = FindObjectsOfType<SetVoxelProperties>();
            new List<SetVoxelProperties>(voxels).ForEach((System.Action<SetVoxelProperties>)((v) =>
            {
                v.ShowNumber = true;
                v.Redraw();
            }));
        }
        else
        {
            SetVoxelProperties[] voxels = FindObjectsOfType<SetVoxelProperties>();
            new List<SetVoxelProperties>(voxels).ForEach((System.Action<SetVoxelProperties>)((v) =>
            {
                v.ShowNumber = false;
                v.Redraw();
            }));
        }
    }

}
