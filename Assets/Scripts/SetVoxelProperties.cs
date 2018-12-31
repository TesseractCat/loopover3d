using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SetVoxelProperties : MonoBehaviour {

    private VoxelSpawner _cube;
    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;
    private Color color = Color.black;
    [SerializeField, FormerlySerializedAs("colorBrightness")]
    private float _colorBrightness = 0.1f;
    [HideInInspector]
    public int Number = 1;
    [HideInInspector]
    public bool Transparent = false;
    [HideInInspector]
    public bool ShowNumber = true;

    void Awake()
    {
        ShowNumber = FindObjectOfType<NumberToggle>().numberEnabled;
        _cube = FindObjectOfType<VoxelSpawner>();
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }
    
	public void SetColor(Color newColor)
    {
        color = newColor;
        _propBlock.SetColor("_Color", color);
        _propBlock.SetColor("_EmissionColor", NumberToColor(Number));
        _propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>(Number.ToString()));
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void SetNumber(int newNumber, bool doRedraw)
    {
        Number = newNumber;
        if (doRedraw)
        {
            _propBlock.SetColor("_Color", NumberToColor(newNumber));
            _propBlock.SetColor("_EmissionColor", NumberToColor(newNumber));
            _propBlock.SetColor("_FirstOutlineColor", OutlineColor());
            if (ShowNumber)
            {
                _propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>(Number.ToString()));
            } else
            {
                _propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>("blank"));
            }
            _renderer.SetPropertyBlock(_propBlock);
        }
    }

    public void DisplayNumber(int displayNumber)
    {
        _propBlock.SetColor("_Color", NumberToColor(Number));
        _propBlock.SetColor("_EmissionColor", NumberToColor(Number));
        _propBlock.SetColor("_FirstOutlineColor", OutlineColor());
        _propBlock.SetTexture("_MainTex", Resources.Load<Texture2D>(displayNumber.ToString()));
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void Redraw()
    {
        SetNumber(Number, true);
    }

    Color NumberToColor(int num)
    {
        int numVoxel = 0;
        for (float xi = 0; xi < _cube.cubeSize; xi++)
        {
            for (float yi = 0; yi < _cube.cubeSize; yi++)
            {
                for (float zi = 0; zi < _cube.cubeSize; zi++)
                {
                    numVoxel += 1;
                    if (num == numVoxel)
                    {
                        if (!Transparent)
                        {
                            return new Color(xi / _cube.cubeSize + _colorBrightness, yi / _cube.cubeSize + _colorBrightness, zi / _cube.cubeSize + _colorBrightness, 1f);
                        } else
                        {
                            return new Color(xi / _cube.cubeSize + _colorBrightness, yi / _cube.cubeSize + _colorBrightness, zi / _cube.cubeSize + _colorBrightness, 0.025f);
                        }
                    }
                }
            }
        }
        return Color.black;
    }

    Color OutlineColor()
    {
        if (Transparent)
        {
            return new Color(0, 0, 0, 0);
        } else
        {
            return new Color(0, 0, 0, 1);
        }
    }
}
