using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVoxelProperties : MonoBehaviour {

    VoxelSpawner cube;
    MaterialPropertyBlock propBlock;
    Renderer renderer;
    public Color color = Color.black;
    public int number = 1;
    public bool transparent = false;
    float colorBrightness = 0.1f;
    public bool showNumber = true;

    void Awake()
    {
        showNumber = FindObjectOfType<NumberToggle>().numberEnabled;
        cube = FindObjectOfType<VoxelSpawner>();
        renderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }
    
	public void SetColor(Color newColor)
    {
        color = newColor;
        propBlock.SetColor("_Color", color);
        propBlock.SetColor("_EmissionColor", NumberToColor(number));
        propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>(number.ToString()));
        renderer.SetPropertyBlock(propBlock);
    }

    public void SetNumber(int newNumber, bool doRedraw)
    {
        number = newNumber;
        if (doRedraw)
        {
            propBlock.SetColor("_Color", NumberToColor(newNumber));
            propBlock.SetColor("_EmissionColor", NumberToColor(newNumber));
            propBlock.SetColor("_FirstOutlineColor", OutlineColor());
            if (showNumber)
            {
                propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>(number.ToString()));
            } else
            {
                propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>("blank"));
            }
            renderer.SetPropertyBlock(propBlock);
        }
    }

    public void ShowNumber(int displayNumber)
    {
        propBlock.SetColor("_Color", NumberToColor(number));
        propBlock.SetColor("_EmissionColor", NumberToColor(number));
        propBlock.SetColor("_FirstOutlineColor", OutlineColor());
        propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>(displayNumber.ToString()));
        renderer.SetPropertyBlock(propBlock);
    }

    public void Redraw()
    {
        SetNumber(number, true);
    }

    Color NumberToColor(int num)
    {
        int numVoxel = 0;
        for (float xi = 0; xi < cube.cubeSize; xi++)
        {
            for (float yi = 0; yi < cube.cubeSize; yi++)
            {
                for (float zi = 0; zi < cube.cubeSize; zi++)
                {
                    numVoxel += 1;
                    if (num == numVoxel)
                    {
                        if (!transparent)
                        {
                            return new Color(xi / cube.cubeSize + colorBrightness, yi / cube.cubeSize + colorBrightness, zi / cube.cubeSize + colorBrightness, 1f);
                        } else
                        {
                            return new Color(xi / cube.cubeSize + colorBrightness, yi / cube.cubeSize + colorBrightness, zi / cube.cubeSize + colorBrightness, 0.025f);
                        }
                    }
                }
            }
        }
        return Color.black;
    }

    Color OutlineColor()
    {
        if (transparent)
        {
            return new Color(0, 0, 0, 0);
        } else
        {
            return new Color(0, 0, 0, 1);
        }
    }
}
