using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoxelSpawner : MonoBehaviour {

    public GameObject voxelPrefab;
    public GameObject nodulePrefab;
    public InputField cubeSizeInput;

    public float noduleOffset = 0.2f;

    //Why is this a float ???
    public float cubeSize;

    public List<GameObject> nodules;

    public Texture2D[] textures;

    public SetVoxelProperties[,,] voxelArray;
    public int[,,] correctNumberArray;

    void Start ()
    {
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 20;

        cubeSize = int.Parse(cubeSizeInput.text);

        voxelArray = new SetVoxelProperties[(int)cubeSize, (int)cubeSize, (int)cubeSize];
        correctNumberArray = new int[(int)cubeSize, (int)cubeSize, (int)cubeSize];

        int numVoxel = 0;
        for (float xi = 0; xi < cubeSize; xi++)
        {
            for (float yi = 0; yi < cubeSize; yi++)
            {
                for (float zi = 0; zi < cubeSize; zi++)
                {
                    numVoxel += 1;
                    GameObject tempVoxel = Instantiate(voxelPrefab, new Vector3(xi, yi, zi), Quaternion.identity, transform);
                    tempVoxel.transform.localPosition = tempVoxel.transform.localPosition;
                    voxelArray[(int)xi, (int)yi, (int)zi] = tempVoxel.GetComponentInChildren<SetVoxelProperties>();
                    correctNumberArray[(int)xi, (int)yi, (int)zi] = numVoxel;

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

        nodules = new List<GameObject>();

        nodules.Add((GameObject)Instantiate(nodulePrefab, new Vector3(0, -1 - noduleOffset, 0), Quaternion.identity, transform));
        nodules.Add((GameObject)Instantiate(nodulePrefab, new Vector3(-1 - noduleOffset, 0, 0), Quaternion.Euler(0,0,-90), transform));
        nodules.Add((GameObject)Instantiate(nodulePrefab, new Vector3(0, 0, -1 - noduleOffset), Quaternion.Euler(90, 0, 0), transform));

        nodules.Add((GameObject)Instantiate(nodulePrefab, new Vector3(cubeSize - 1, cubeSize + noduleOffset, cubeSize - 1), Quaternion.Euler(180, 0, 0), transform));
        nodules.Add((GameObject)Instantiate(nodulePrefab, new Vector3(cubeSize + noduleOffset, cubeSize - 1, cubeSize - 1), Quaternion.Euler(0, 0, 90), transform));
        nodules.Add((GameObject)Instantiate(nodulePrefab, new Vector3(cubeSize - 1, cubeSize - 1, cubeSize + noduleOffset), Quaternion.Euler(-90, 0, 0), transform));

        //transform.position = new Vector3(-1f, -1f, -1f);
        transform.localScale = new Vector3((1 / cubeSize) * 2.5f, (1 / cubeSize) * 2.5f, (1 / cubeSize) * 2.5f);
        GameObject center = new GameObject();
        center.transform.parent = transform;
        center.transform.localPosition = new Vector3(cubeSize / 2 - 0.5f, cubeSize / 2 - 0.5f, cubeSize / 2 - 0.5f);
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
