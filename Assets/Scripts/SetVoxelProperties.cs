﻿using UnityEngine;
using UnityEngine.Serialization;

public class SetVoxelProperties : MonoBehaviour
{
    private Color _color = Color.black;

    [SerializeField] [FormerlySerializedAs("colorBrightness")]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private float _colorBrightness = 0.1f;

    private VoxelSpawner _cube;
    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;

    [HideInInspector] public int Number = 1;
    [HideInInspector] public bool ShowNumber = true;
    [HideInInspector] public bool Transparent = false;

    private void Awake()
    {
        ShowNumber = FindObjectOfType<NumberToggle>().NumberEnabled;
        _cube = FindObjectOfType<VoxelSpawner>();
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    public void SetColor(Color newColor)
    {
        _color = newColor;
        _propBlock.SetColor("_Color", _color);
        _propBlock.SetColor("_EmissionColor", NumberToColor(Number));
        _propBlock.SetTexture(
            "_MainTex",
            Resources.Load<Texture2D>(Number.ToString())
        );
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void SetNumber(int newNumber, bool doRedraw)
    {
        Number = newNumber;
        if (!doRedraw) return;
        _propBlock.SetColor("_Color", NumberToColor(newNumber));
        _propBlock.SetColor("_EmissionColor", NumberToColor(newNumber));
        _propBlock.SetColor("_FirstOutlineColor", OutlineColor());
        _propBlock.SetTexture(
            "_MainTex",
            ShowNumber
                ? Resources.Load<Texture2D>(Number.ToString())
                : Resources.Load<Texture2D>("blank")
        );

        _renderer.SetPropertyBlock(_propBlock);
    }

    public void DisplayNumber(int displayNumber)
    {
        _propBlock.SetColor("_Color", NumberToColor(Number));
        _propBlock.SetColor("_EmissionColor", NumberToColor(Number));
        _propBlock.SetColor("_FirstOutlineColor", OutlineColor());
        _propBlock.SetTexture(
            "_MainTex",
            Resources.Load<Texture2D>(displayNumber.ToString())
        );
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void Redraw()
    {
        SetNumber(Number, true);
    }

    private Color NumberToColor(int num)
    {
        var numVoxel = 0;
        for (float xi = 0; xi < _cube.CubeSize; xi++)
        for (float yi = 0; yi < _cube.CubeSize; yi++)
        for (float zi = 0; zi < _cube.CubeSize; zi++)
        {
            numVoxel += 1;
            if (num == numVoxel)
            {
                if (!Transparent)
                    return new Color(
                        xi / _cube.CubeSize + _colorBrightness,
                        yi / _cube.CubeSize + _colorBrightness,
                        zi / _cube.CubeSize + _colorBrightness,
                        1f
                    );
                return new Color(
                    xi / _cube.CubeSize + _colorBrightness,
                    yi / _cube.CubeSize + _colorBrightness,
                    zi / _cube.CubeSize + _colorBrightness,
                    0.025f
                );
            }
        }

        return Color.black;
    }

    private Color OutlineColor()
    {
        return Transparent
            ? new Color(0, 0, 0, 0)
            : new Color(0, 0, 0, 1);
    }
}