using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VoxelSpawner : MonoBehaviour {

    [SerializeField, FormerlySerializedAs("voxelPrefab")]
    private GameObject _voxelPrefab = null;
    [SerializeField, FormerlySerializedAs("nodulePrefab")]
    private GameObject _nodulePrefab = null;
    [SerializeField, FormerlySerializedAs("cubeSizeInput")]
    private InputField _cubeSizeInput = null;

    [SerializeField, FormerlySerializedAs("noduleOffset")]
    private float _noduleOffset = 0.2f;

    //Why is this a float ???
    public float CubeSize;

    public List<GameObject> Nodules;

    [SerializeField, FormerlySerializedAs("textures")]
    private Texture2D[] _textures;

    public SetVoxelProperties[,,] VoxelArray;
    public int[,,] CorrectNumberArray { get; private set; }

    void Start ()
    {
        CubeSize = int.Parse(_cubeSizeInput.text);

        VoxelArray = new SetVoxelProperties[(int)CubeSize, (int)CubeSize, (int)CubeSize];
        CorrectNumberArray = new int[(int)CubeSize, (int)CubeSize, (int)CubeSize];

        int numVoxel = 0;
        for (float xi = 0; xi < CubeSize; xi++)
        {
            for (float yi = 0; yi < CubeSize; yi++)
            {
                for (float zi = 0; zi < CubeSize; zi++)
                {
                    numVoxel += 1;
                    GameObject tempVoxel = Instantiate(_voxelPrefab, new Vector3(xi, yi, zi), Quaternion.identity, transform);
                    tempVoxel.transform.localPosition = tempVoxel.transform.localPosition;
                    VoxelArray[(int)xi, (int)yi, (int)zi] = tempVoxel.GetComponentInChildren<SetVoxelProperties>();
                    CorrectNumberArray[(int)xi, (int)yi, (int)zi] = numVoxel;

                    //Instanced colors
                    if (numVoxel <= 999)
                    {
                        tempVoxel.GetComponentInChildren<SetVoxelProperties>().SetNumber(numVoxel, true);
                        tempVoxel.GetComponentInChildren<ShowCorrectNumber>().CorrectNumber = numVoxel;
                    }
                    //tempVoxel.GetComponentInChildren<SetVoxelProperties>().SetColor(new Color(xi / cubeSize + 0.1f, yi / cubeSize + 0.1f, zi / cubeSize + 0.1f, 1f));
                    //renderer.material = new Material(renderer.material);
                    //renderer.material.color = new Color(xi / cubeSize, yi / cubeSize, zi / cubeSize);

                    //tempVoxel.GetComponentInChildren<ShowCorrectNumber>().actualNumber = numVoxel;
                    //tempVoxel.GetComponentInChildren<ShowCorrectNumber>().actualColor = new Color(xi / cubeSize + 0.1f, yi / cubeSize + 0.1f, zi / cubeSize + 0.1f);
                }
            }
        }

        Nodules = new List<GameObject>();

        Nodules.Add((GameObject)Instantiate(_nodulePrefab, new Vector3(0, -1 - _noduleOffset, 0), Quaternion.identity, transform));
        Nodules.Add((GameObject)Instantiate(_nodulePrefab, new Vector3(-1 - _noduleOffset, 0, 0), Quaternion.Euler(0,0,-90), transform));
        Nodules.Add((GameObject)Instantiate(_nodulePrefab, new Vector3(0, 0, -1 - _noduleOffset), Quaternion.Euler(90, 0, 0), transform));

        Nodules.Add((GameObject)Instantiate(_nodulePrefab, new Vector3(CubeSize - 1, CubeSize + _noduleOffset, CubeSize - 1), Quaternion.Euler(180, 0, 0), transform));
        Nodules.Add((GameObject)Instantiate(_nodulePrefab, new Vector3(CubeSize + _noduleOffset, CubeSize - 1, CubeSize - 1), Quaternion.Euler(0, 0, 90), transform));
        Nodules.Add((GameObject)Instantiate(_nodulePrefab, new Vector3(CubeSize - 1, CubeSize - 1, CubeSize + _noduleOffset), Quaternion.Euler(-90, 0, 0), transform));

        //transform.position = new Vector3(-1f, -1f, -1f);
        transform.localScale = new Vector3((1 / CubeSize) * 2.5f, (1 / CubeSize) * 2.5f, (1 / CubeSize) * 2.5f);
        GameObject center = new GameObject();
        center.transform.parent = transform;
        center.transform.localPosition = new Vector3(CubeSize / 2 - 0.5f, CubeSize / 2 - 0.5f, CubeSize / 2 - 0.5f);
        center.transform.parent = null;
        transform.position = -center.transform.position;
        Destroy(center);
        //transform.parent = center.transform;
        //center.transform.position = Vector3.zero;
        //transform.parent = null;
        //Destroy(center);

        GetComponent<VoxelMover>().base64MoveEncode = "";
	}

    public void ResetCube()
    {
        GetComponent<VoxelMover>().timeDisplay.text = "Time: 0";
        GetComponent<VoxelMover>().movesDisplay.text = "Moves: 0";
        GetComponent<VoxelMover>().MPSDisplay.text = "MPS: 0";
        GetComponent<VoxelMover>().Restart();
        transform.localScale = Vector3.one;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        for (int c = 0; c < transform.childCount; c++)
        {
            if (transform.GetChild(c).tag == "Voxel" || transform.GetChild(c).tag == "Nodule")
            {
                Destroy(transform.GetChild(c).gameObject);
            }
        }
        GetComponent<VoxelMover>().AmountCut = 0;
        Start();
    }

}
