using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VoxelMover : MonoBehaviour {
    [SerializeField, FormerlySerializedAs("VoxelHighlighter")]
    private GameObject _voxelHighlighter = default(GameObject);
    public Text timeDisplay;
    public Text movesDisplay;
    public Text MPSDisplay;
    [SerializeField, FormerlySerializedAs("timerButtonText")]
    private Text _timerButtonText = default(Text);
    private float _cubeSize;

    [SerializeField, FormerlySerializedAs("sliceToggle")]
    private Toggle _sliceToggle = default(Toggle);
    private bool _alternateSliceDir = false;

    private Vector3 _newPos;
    private Vector3 _newNorm;
    private Vector3 _highlightedVoxel;

    private bool _doTiming = false;
    private float _startTime = 0;
    private int _moves = 0;

    public int AmountCut = 0;
    private Vector3 _currentCutDir = Vector3.zero;

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
        if (_doTiming)
        {
            timeDisplay.text = "Time: " + (Time.time - _startTime).ToString();
            movesDisplay.text = "Moves: " + (_moves).ToString();
            MPSDisplay.text = "MPS: " + (_moves/ (Time.time - _startTime)).ToString();
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.tag == "Voxel")
            {
                _newPos = hit.transform.position;
                _newNorm = hit.normal;
                _highlightedVoxel = hit.transform.localPosition;
                _voxelHighlighter.SetActive(true);
            } else
            {
                _voxelHighlighter.SetActive(false);
            }
        } else
        {
            _voxelHighlighter.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _alternateSliceDir = !_alternateSliceDir;
        }

        if (_voxelHighlighter.activeSelf && Input.GetMouseButtonDown(0))
        {
            _cubeSize = vs.CubeSize;
            for (int c = 0; c < transform.childCount; c++)
            {
                if (transform.GetChild(c).tag == "Moved")
                {
                    Debug.Log("BREAK AVERTED");
                    return;
                }
            }

            if (_doTiming == false)
            {
                base64MoveEncode = Base64Encode((int)_cubeSize);
                _timerButtonText.text = "Stop Timer";
                _startTime = Time.time;
                _doTiming = true;
                _moves = 0;
            }
            
            Vector3 localNorm = transform.InverseTransformDirection(_newNorm);
            _moves += 1;

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

            if (!_sliceToggle.isOn)
            {
                PushRow(_highlightedVoxel, -localNorm, true);
            } else
            {
                PushSlice(_highlightedVoxel, -localNorm);
            }

            //Base64MoveEncoder
            base64MoveEncode += Base64Encode((int)_highlightedVoxel.x) + Base64Encode((int)_highlightedVoxel.y) + Base64Encode((int)_highlightedVoxel.z);
            for (int i = 0; i < directionArray.Length; i++)
            {
                if (directionArray[i] == -localNorm)
                {
                    base64MoveEncode += Base64Encode(i);
                }
            }
            

            bool stopTimer = true;
            for (int xi = 0; xi < _cubeSize; xi++)
            {
                for (int yi = 0; yi < _cubeSize; yi++)
                {
                    for (int zi = 0; zi < _cubeSize; zi++)
                    {
                        if (vs.VoxelArray[xi, yi, zi].Number != vs.CorrectNumberArray[xi, yi, zi])
                        {
                            stopTimer = false;
                        }
                    }
                }
            }
            if (stopTimer)
            {
                Restart();
                _timerButtonText.text = "Cube Solved!";
            }
        }

        //Hardcoded values uwu
        if (!_sliceToggle.isOn)
        {
            _voxelHighlighter.transform.position = Vector3.Lerp(_voxelHighlighter.transform.position, _newPos, Time.deltaTime * 30);
            _voxelHighlighter.transform.up = Vector3.Lerp(_voxelHighlighter.transform.up, _newNorm, Time.deltaTime * 50);
        } else
        {
            _voxelHighlighter.transform.position = Vector3.Lerp(_voxelHighlighter.transform.position, _newPos, Time.deltaTime * 30);
            //VoxelHighlighter.transform.up = Vector3.Lerp(VoxelHighlighter.transform.up, newNorm, Time.deltaTime * 50);

            Vector3 localNorm = transform.InverseTransformDirection(_newNorm);
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

            if (!_alternateSliceDir)
            {
                _voxelHighlighter.transform.rotation = Quaternion.LookRotation(
                    Vector3.Lerp(_voxelHighlighter.transform.forward, transform.TransformDirection(highlightRight), Time.deltaTime * 50),
                    Vector3.Lerp(_voxelHighlighter.transform.up, _newNorm, Time.deltaTime * 50));
            } else
            {
                _voxelHighlighter.transform.rotation = Quaternion.LookRotation(
                    Vector3.Lerp(_voxelHighlighter.transform.forward, transform.TransformDirection(highlightUp), Time.deltaTime * 50),
                    Vector3.Lerp(_voxelHighlighter.transform.up, _newNorm, Time.deltaTime * 50));
            }

        }
    }

    public void Restart()
    {
        _doTiming = false;
        _moves = 0;
        _timerButtonText.text = "Timer Stopped";
    }

    public void ToggleSliceMarkers()
    {
        if (_sliceToggle.isOn)
        {
            _voxelHighlighter.transform.Find("SliceMarkers").gameObject.SetActive(true);
        } else
        {
            _voxelHighlighter.transform.Find("SliceMarkers").gameObject.SetActive(false);
        }
    }

    public void CutForward(Vector3 dir)
    {
        _currentCutDir = dir;
        _cubeSize = vs.CubeSize;
        if (AmountCut == ((int)_cubeSize - 1))
        {
            return;
        }

        List<SetVoxelProperties> objectsToDeactivate = new List<SetVoxelProperties>();
        for (int xi = 0; xi < _cubeSize; xi++)
        {
            for (int yi = 0; yi < _cubeSize; yi++)
            {
                for (int zi = 0; zi < _cubeSize; zi++)
                {
                    SetVoxelProperties beforeVoxel = null;
                    try
                    {
                        beforeVoxel = vs.VoxelArray[xi - Mathf.RoundToInt(dir.x), yi - Mathf.RoundToInt(dir.y), zi - Mathf.RoundToInt(dir.z)];
                    }
                    catch
                    {
                        objectsToDeactivate.Add(vs.VoxelArray[xi, yi, zi]);
                        continue;
                    }

                    if (beforeVoxel.Transparent)
                    {
                        objectsToDeactivate.Add(vs.VoxelArray[xi, yi, zi]);
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

        AmountCut += 1;
    }

    public void CutBackward(Vector3 dir, bool resetNodulesOnZero)
    {
        if (AmountCut == 0)
        {
            return;
        }


        List<SetVoxelProperties> objectsToActivate = new List<SetVoxelProperties>();
        for (int xi = 0; xi < _cubeSize; xi++)
        {
            for (int yi = 0; yi < _cubeSize; yi++)
            {
                for (int zi = 0; zi < _cubeSize; zi++)
                {
                    SetVoxelProperties beforeVoxel = null;
                    try
                    {
                        beforeVoxel = vs.VoxelArray[xi + Mathf.RoundToInt(dir.x), yi + Mathf.RoundToInt(dir.y), zi + Mathf.RoundToInt(dir.z)];
                    } catch
                    {
                        continue;
                    }

                    if (!beforeVoxel.Transparent)
                    {
                        objectsToActivate.Add(vs.VoxelArray[xi, yi, zi]);
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

        AmountCut -= 1;

        if (AmountCut == 0 && resetNodulesOnZero)
        {
            GetComponent<VoxelSpawner>().Nodules.ForEach((n) =>
            {
                n.SetActive(true);
            });
        }
    }

    void PushRow(Vector3 touched, Vector3 dir, bool fancy)
    {
        _cubeSize = vs.CubeSize;

        for (int i = 0; i < _cubeSize + 1; i++)
        {
            if (FindVoxelByPos(touched - (dir * i)) == null)
            {
                touched = touched - (dir * (i - 1));
                break;
            }
        }

        List<int> tempNumArray = new List<int>();
        for (int i = 0; i < _cubeSize; i++)
        {
            Vector3 pos = (touched + (dir * i));
            tempNumArray.Add(vs.VoxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].Number);
            //Debug.Log(tempNumArray[tempNumArray.Count - 1]);
        }
        for (int i = 0; i < _cubeSize; i++)
        {
            Vector3 pos = (touched + (dir * i));
            if (i == 0)
            {
                vs.VoxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].SetNumber(tempNumArray[tempNumArray.Count-1], fancy);
            } else
            {
                vs.VoxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].SetNumber(tempNumArray[i-1], fancy);
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

        if (!_alternateSliceDir)
        {
            sliceDir = highlightUp;
        }
        else
        {
            sliceDir = highlightRight;
        }

        _cubeSize = vs.CubeSize;

        for (int i = 0; i < _cubeSize + 1; i++)
        {
            if (FindVoxelByPos(_highlightedVoxel - (sliceDir * i)) == null)
            {
                _highlightedVoxel = _highlightedVoxel - (sliceDir * (i - 1));
                break;
            }
        }

        for (int i = 0; i < _cubeSize; i++)
        {
            PushRow(_highlightedVoxel + sliceDir * i, dir, true);
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
            return vs.VoxelArray[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)].transform;
        } catch
        {
            return null;
        }
    }

    public void Scramble()
    {
        if (AmountCut != 0)
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
