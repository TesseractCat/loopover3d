﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineToggle : MonoBehaviour {

    bool outlineEnabled = true;

    public void ToggleOutlines()
    {
        outlineEnabled = !outlineEnabled;
        if (outlineEnabled)
        {
            FindObjectOfType<SetVoxelProperties>().GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", 0.02f);
        } else
        {
            FindObjectOfType<SetVoxelProperties>().GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", 0f);
        }
    }

}
