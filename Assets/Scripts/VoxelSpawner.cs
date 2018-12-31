using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VoxelSpawner : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("cubeSizeInput")]
    private readonly InputField _cubeSizeInput = null;

    [SerializeField] [FormerlySerializedAs("noduleOffset")]
    private readonly float _noduleOffset = 0.2f;

    [SerializeField] [FormerlySerializedAs("nodulePrefab")]
    private readonly GameObject _nodulePrefab = null;

    [SerializeField] [FormerlySerializedAs("voxelPrefab")]
    private readonly GameObject _voxelPrefab = null;

    [SerializeField] [FormerlySerializedAs("textures")]
    private Texture2D[] _textures;

    public int CubeSize;

    public List<GameObject> Nodules;

    public SetVoxelProperties[,,] VoxelArray;
    public int[,,] CorrectNumberArray { get; private set; }

    private void Start()
    {
        CubeSize = int.Parse(_cubeSizeInput.text);

        VoxelArray = new SetVoxelProperties[CubeSize, CubeSize, CubeSize];
        CorrectNumberArray = new int[CubeSize, CubeSize, CubeSize];

        var numVoxel = 0;
        for (var xi = 0; xi < CubeSize; xi++)
        for (var yi = 0; yi < CubeSize; yi++)
        for (var zi = 0; zi < CubeSize; zi++)
        {
            numVoxel += 1;
            var tempVoxel = Instantiate(
                _voxelPrefab,
                new Vector3(xi, yi, zi),
                Quaternion.identity,
                transform
            );
            tempVoxel.transform.localPosition =
                tempVoxel.transform.localPosition;
            VoxelArray[xi, yi, zi] =
                tempVoxel.GetComponentInChildren<SetVoxelProperties>();
            CorrectNumberArray[xi, yi, zi] = numVoxel;

            //Instanced colors
            if (numVoxel <= 999)
            {
                tempVoxel.GetComponentInChildren<SetVoxelProperties>()
                    .SetNumber(numVoxel, true);
                tempVoxel.GetComponentInChildren<ShowCorrectNumber>()
                    .CorrectNumber = numVoxel;
            }
        }

        Nodules = new List<GameObject>();

        Nodules.Add(
            Instantiate(
                _nodulePrefab,
                new Vector3(0, -1 - _noduleOffset, 0),
                Quaternion.identity,
                transform
            )
        );
        Nodules.Add(
            Instantiate(
                _nodulePrefab,
                new Vector3(-1 - _noduleOffset, 0, 0),
                Quaternion.Euler(0, 0, -90),
                transform
            )
        );
        Nodules.Add(
            Instantiate(
                _nodulePrefab,
                new Vector3(0, 0, -1 - _noduleOffset),
                Quaternion.Euler(90, 0, 0),
                transform
            )
        );

        Nodules.Add(
            Instantiate(
                _nodulePrefab,
                new Vector3(
                    CubeSize - 1,
                    CubeSize + _noduleOffset,
                    CubeSize - 1
                ),
                Quaternion.Euler(180, 0, 0),
                transform
            )
        );
        Nodules.Add(
            Instantiate(
                _nodulePrefab,
                new Vector3(
                    CubeSize + _noduleOffset,
                    CubeSize - 1,
                    CubeSize - 1
                ),
                Quaternion.Euler(0, 0, 90),
                transform
            )
        );
        Nodules.Add(
            Instantiate(
                _nodulePrefab,
                new Vector3(
                    CubeSize - 1,
                    CubeSize - 1,
                    CubeSize + _noduleOffset
                ),
                Quaternion.Euler(-90, 0, 0),
                transform
            )
        );

        transform.localScale = new Vector3(
            1f / CubeSize * 2.5f,
            1f / CubeSize * 2.5f,
            1f / CubeSize * 2.5f
        );
        var center = new GameObject();
        center.transform.parent = transform;
        center.transform.localPosition = new Vector3(
            CubeSize / 2f - 0.5f,
            CubeSize / 2f - 0.5f,
            CubeSize / 2f - 0.5f
        );
        center.transform.parent = null;
        transform.position = -center.transform.position;
        Destroy(center);

        GetComponent<VoxelMover>().base64MoveEncode = "";
    }

    public void ResetCube()
    {
        GetComponent<VoxelMover>().TimeDisplay.text = "Time: 0";
        GetComponent<VoxelMover>().MovesDisplay.text = "Moves: 0";
        GetComponent<VoxelMover>().MPSDisplay.text = "MPS: 0";
        GetComponent<VoxelMover>().Restart();
        transform.localScale = Vector3.one;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        for (var c = 0; c < transform.childCount; c++)
            if (transform.GetChild(c).tag == "Voxel" ||
                transform.GetChild(c).tag == "Nodule")
                Destroy(transform.GetChild(c).gameObject);

        GetComponent<VoxelMover>().AmountCut = 0;
        Start();
    }
}