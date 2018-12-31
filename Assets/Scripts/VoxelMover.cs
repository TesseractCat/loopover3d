using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VoxelMover : MonoBehaviour {

    public GameObject VoxelHighlighter;
    public Text timeDisplay;
    public Text movesDisplay;
    public Text MPSDisplay;
    public Text timerButtonText;
    float cubeSize;

    public Toggle sliceToggle;
    bool alternateSliceDir = false;

    Vector3 newPos;
    Vector3 newNorm;
    Vector3 highlightedVoxel;

    bool doTiming = false;
    float startTime = 0;
    int moves = 0;

    public int amountCut = 0;
    Vector3 currentCutDir = Vector3.zero;

    VoxelSpawner vs;
    Vector3[] directionArray = new[]
            {
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1)
            };

    public string base64MoveEncode = "";

    void Start()
    {
        vs = FindObjectOfType<VoxelSpawner>();
    }

    void Update () {
        if (doTiming)
        {
            timeDisplay.text = "Time: " + (Time.time - startTime).ToString();
            movesDisplay.text = "Moves: " + (moves).ToString();
            MPSDisplay.text = "MPS: " + (moves/ (Time.time - startTime)).ToString();
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.tag == "Voxel")
            {
                newPos = hit.transform.position;
                newNorm = hit.normal;
                highlightedVoxel = hit.transform.localPosition;
                VoxelHighlighter.SetActive(true);
            } else
            {
                VoxelHighlighter.SetActive(false);
            }
        } else
        {
            VoxelHighlighter.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            alternateSliceDir = !alternateSliceDir;
        }

        if (VoxelHighlighter.activeSelf && Input.GetMouseButtonDown(0))
        {
            cubeSize = vs.cubeSize;
            for (int c = 0; c < transform.childCount; c++)
            {
                if (transform.GetChild(c).tag == "Moved")
                {
                    Debug.Log("BREAK AVERTED");
                    return;
                }
            }

            if (doTiming == false)
            {
                base64MoveEncode = Base64Encode((int)cubeSize);
                timerButtonText.text = "Stop Timer";
                startTime = Time.time;
                doTiming = true;
                moves = 0;
            }
            
            Vector3 localNorm = transform.InverseTransformDirection(newNorm);
            moves += 1;

            bool tempReturn = true;
            for (int d = 0; d < directionArray.Length; d++)
            {
                if (-localNorm == directionArray[d])
                {
                    tempReturn = false;
                }
            }
            if (tempReturn)
            {
                Debug.Log("WEIRD DIRECTION: " + -localNorm);
                return;
            }

            if (!sliceToggle.isOn)
            {
                PushRow(highlightedVoxel, -localNorm, true);
            } else
            {
                PushSlice(highlightedVoxel, -localNorm);
            }

            //Base64MoveEncoder
            base64MoveEncode += Base64Encode((int)highlightedVoxel.x) + Base64Encode((int)highlightedVoxel.y) + Base64Encode((int)highlightedVoxel.z);
            for (int i = 0; i < directionArray.Length; i++)
            {
                if (directionArray[i] == -localNorm)
                {
                    base64MoveEncode += Base64Encode(i);
                }
            }
            

            bool stopTimer = true;
            for (int xi = 0; xi < cubeSize; xi++)
            {
                for (int yi = 0; yi < cubeSize; yi++)
                {
                    for (int zi = 0; zi < cubeSize; zi++)
                    {
                        if (vs.voxelArray[xi, yi, zi].Number != vs.correctNumberArray[xi, yi, zi])
                        {
                            stopTimer = false;
                        }
                    }
                }
            }
            if (stopTimer)
            {
                Restart();
                timerButtonText.text = "Cube Solved!";
            }
        }

        //Hardcoded values uwu
        if (!sliceToggle.isOn)
        {
            VoxelHighlighter.transform.position = Vector3.Lerp(VoxelHighlighter.transform.position, newPos, Time.deltaTime * 30);
            VoxelHighlighter.transform.up = Vector3.Lerp(VoxelHighlighter.transform.up, newNorm, Time.deltaTime * 50);
        } else
        {
            VoxelHighlighter.transform.position = Vector3.Lerp(VoxelHighlighter.transform.position, newPos, Time.deltaTime * 30);
            //VoxelHighlighter.transform.up = Vector3.Lerp(VoxelHighlighter.transform.up, newNorm, Time.deltaTime * 50);

            Vector3 localNorm = transform.InverseTransformDirection(newNorm);
            Vector3 highlightUp = Vector3.zero;
            Vector3 highlightRight = Vector3.zero;
            if (Mathf.RoundToInt(vec3Abs(localNorm).x) == 1)
            {
                highlightUp = new Vector3(0, 1, 0);
                highlightRight = new Vector3(0, 0, 1);
            }
            else if (Mathf.RoundToInt(vec3Abs(localNorm).y) == 1)
            {
                highlightUp = new Vector3(0, 0, 1);
                highlightRight = new Vector3(1, 0, 0);
            }
            else if (Mathf.RoundToInt(vec3Abs(localNorm).z) == 1)
            {
                highlightUp = new Vector3(0, 1, 0);
                highlightRight = new Vector3(1, 0, 0);
            }

            if (!alternateSliceDir)
            {
                VoxelHighlighter.transform.rotation = Quaternion.LookRotation(
                    Vector3.Lerp(VoxelHighlighter.transform.forward, transform.TransformDirection(highlightRight), Time.deltaTime * 50),
                    Vector3.Lerp(VoxelHighlighter.transform.up, newNorm, Time.deltaTime * 50));
            } else
            {
                VoxelHighlighter.transform.rotation = Quaternion.LookRotation(
                    Vector3.Lerp(VoxelHighlighter.transform.forward, transform.TransformDirection(highlightUp), Time.deltaTime * 50),
                    Vector3.Lerp(VoxelHighlighter.transform.up, newNorm, Time.deltaTime * 50));
            }

        }
    }

    public void Restart()
    {
        doTiming = false;
        moves = 0;
        timerButtonText.text = "Timer Stopped";
    }

    public void ToggleSliceMarkers()
    {
        if (sliceToggle.isOn)
        {
            VoxelHighlighter.transform.Find("SliceMarkers").gameObject.SetActive(true);
        } else
        {
            VoxelHighlighter.transform.Find("SliceMarkers").gameObject.SetActive(false);
        }
    }

    public void CutForward(Vector3 dir)
    {
        currentCutDir = dir;
        cubeSize = vs.cubeSize;
        if (amountCut == ((int)cubeSize - 1))
        {
            return;
        }

        List<SetVoxelProperties> objectsToDeactivate = new List<SetVoxelProperties>();
        for (int xi = 0; xi < cubeSize; xi++)
        {
            for (int yi = 0; yi < cubeSize; yi++)
            {
                for (int zi = 0; zi < cubeSize; zi++)
                {
                    SetVoxelProperties beforeVoxel = null;
                    try
                    {
                        beforeVoxel = vs.voxelArray[xi - Mathf.RoundToInt(dir.x), yi - Mathf.RoundToInt(dir.y), zi - Mathf.RoundToInt(dir.z)];
                    }
                    catch
                    {
                        objectsToDeactivate.Add(vs.voxelArray[xi, yi, zi]);
                        continue;
                    }

                    if (beforeVoxel.Transparent)
                    {
                        objectsToDeactivate.Add(vs.voxelArray[xi, yi, zi]);
                    }
                }
            }
        }
        objectsToDeactivate.ForEach((Obj) =>
        {
            Obj.transform.parent.GetComponent<Collider>().enabled = false;
            Obj.Transparent = true;
            Obj.Redraw();
        });

        amountCut += 1;
    }

    public void CutBackward(Vector3 dir, bool resetNodulesOnZero)
    {
        if (amountCut == 0)
        {
            return;
        }


        List<SetVoxelProperties> objectsToActivate = new List<SetVoxelProperties>();
        for (int xi = 0; xi < cubeSize; xi++)
        {
            for (int yi = 0; yi < cubeSize; yi++)
            {
                for (int zi = 0; zi < cubeSize; zi++)
                {
                    SetVoxelProperties beforeVoxel = null;
                    try
                    {
                        beforeVoxel = vs.voxelArray[xi + Mathf.RoundToInt(dir.x), yi + Mathf.RoundToInt(dir.y), zi + Mathf.RoundToInt(dir.z)];
                    } catch
                    {
                        continue;
                    }

                    if (!beforeVoxel.Transparent)
                    {
                        objectsToActivate.Add(vs.voxelArray[xi, yi, zi]);
                    }
                }
            }
        }
        objectsToActivate.ForEach((Obj) =>
        {
            Obj.transform.parent.GetComponent<Collider>().enabled = true;
            Obj.Transparent = false;
            Obj.Redraw();
        });

        amountCut -= 1;

        if (amountCut == 0 && resetNodulesOnZero)
        {
            GetComponent<VoxelSpawner>().nodules.ForEach((n) =>
            {
                n.SetActive(true);
            });
        }
    }

    void PushRow(Vector3 touched, Vector3 dir, bool fancy)
    {
        cubeSize = vs.cubeSize;

        for (int i = 0; i < cubeSize + 1; i++)
        {
            if (FindVoxelByPos(touched - (dir * i)) == null)
            {
                touched = touched - (dir * (i - 1));
                break;
            }
        }

        List<int> tempNumArray = new List<int>();
        for (int i = 0; i < cubeSize; i++)
        {
            Vector3 pos = (touched + (dir * i));
            tempNumArray.Add(vs.voxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].Number);
            //Debug.Log(tempNumArray[tempNumArray.Count - 1]);
        }
        for (int i = 0; i < cubeSize; i++)
        {
            Vector3 pos = (touched + (dir * i));
            if (i == 0)
            {
                vs.voxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].SetNumber(tempNumArray[tempNumArray.Count-1], fancy);
            } else
            {
                vs.voxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].SetNumber(tempNumArray[i-1], fancy);
            }
        }

        for (int c = 0; c < transform.childCount; c++)
        {
            if (transform.GetChild(c).tag == "Moved")
            {
                transform.GetChild(c).tag = "Voxel";
            }
        }
    }

    void PushSlice(Vector3 touched, Vector3 dir)
    {
        //HACKY
        Vector3 highlightUp = Vector3.zero;
        Vector3 highlightRight = Vector3.zero;
        if (Mathf.RoundToInt(vec3Abs(dir).x) == 1)
        {
            highlightUp = new Vector3(0, 1, 0);
            highlightRight = new Vector3(0, 0, 1);
        }
        else if (Mathf.RoundToInt(vec3Abs(dir).y) == 1)
        {
            highlightUp = new Vector3(0, 0, 1);
            highlightRight = new Vector3(1, 0, 0);
        }
        else if (Mathf.RoundToInt(vec3Abs(dir).z) == 1)
        {
            highlightUp = new Vector3(0, 1, 0);
            highlightRight = new Vector3(1, 0, 0);
        }

        Vector3 sliceDir = Vector3.zero;

        if (!alternateSliceDir)
        {
            sliceDir = highlightUp;
        }
        else
        {
            sliceDir = highlightRight;
        }

        cubeSize = vs.cubeSize;

        for (int i = 0; i < cubeSize + 1; i++)
        {
            if (FindVoxelByPos(highlightedVoxel - (sliceDir * i)) == null)
            {
                highlightedVoxel = highlightedVoxel - (sliceDir * (i - 1));
                break;
            }
        }

        for (int i = 0; i < cubeSize; i++)
        {
            PushRow(highlightedVoxel + sliceDir * i, dir, true);
        }
    }

    Vector3 floorVec3ToInt(Vector3 in_vec)
    {
        return new Vector3(Mathf.FloorToInt(in_vec.x), Mathf.FloorToInt(in_vec.y), Mathf.FloorToInt(in_vec.z));
    }

    Vector3 vec3Abs(Vector3 in_vec)
    {
        return new Vector3(Mathf.Abs(in_vec.x), Mathf.Abs(in_vec.y), Mathf.Abs(in_vec.z));
    }

    Transform FindVoxelByPos(Vector3 pos)
    {
        /*for (int c = 0; c < transform.childCount; c++)
        {
            if (transform.GetChild(c).transform.localPosition == pos && transform.GetChild(c).tag == "Voxel")
            {
                return transform.GetChild(c);
            }
        }
        return null;*/

        try
        {
            return vs.voxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].transform;
        } catch
        {
            return null;
        }
    }

    public void Scramble()
    {
        if (amountCut != 0)
        {
            return;
        }

        for (int i = 0; i < 1000; i++)
        {
            ScrambleOne(true);
        }
    }

    void ScrambleOne(bool end)
    {
        int randVoxel = UnityEngine.Random.Range(0, transform.childCount);
        if (transform.GetChild(randVoxel).tag == "Voxel")
        {
            //if (!sliceToggle.isOn)
            //{
                PushRow(transform.GetChild(randVoxel).localPosition, directionArray[UnityEngine.Random.Range(0, directionArray.Length - 1)], end);
            //} else
            //{
            //    PushSlice(transform.GetChild(randVoxel).localPosition, directionArray[UnityEngine.Random.Range(0, directionArray.Length - 1)]);
            //}
        }
    }

    public string Base64Encode(int num)
    {
        string base64Table = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        return base64Table[num].ToString();
    }
}
