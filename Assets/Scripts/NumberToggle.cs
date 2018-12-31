using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberToggle : MonoBehaviour
{

    public bool numberEnabled = true;

    public void ToggleNumbers()
    {
        numberEnabled = !numberEnabled;
        if (numberEnabled)
        {
            SetVoxelProperties[] voxels = FindObjectsOfType<SetVoxelProperties>();
            new List<SetVoxelProperties>(voxels).ForEach((v) =>
            {
                v.showNumber = true;
                v.Redraw();
            });
        }
        else
        {
            SetVoxelProperties[] voxels = FindObjectsOfType<SetVoxelProperties>();
            new List<SetVoxelProperties>(voxels).ForEach((v) =>
            {
                v.showNumber = false;
                v.Redraw();
            });
        }
    }

}
