using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VoxelMover : MonoBehaviour
{
    private readonly Vector3[] _directionArray =
    {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1)
    };

    private bool _alternateSliceDir;

    private float _cubeSize;

    private bool _doTiming;
    private Vector3 _highlightedVoxel;
    private int _moves;
    private Vector3 _newNorm;

    private Vector3 _newPos;

    [SerializeField] [FormerlySerializedAs("sliceToggle")]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private Toggle _sliceToggle = default(Toggle);

    private float _startTime;

    [SerializeField] [FormerlySerializedAs("timerButtonText")]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private Text _timerButtonText = default(Text);

    [SerializeField] [FormerlySerializedAs("VoxelHighlighter")]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private GameObject _voxelHighlighter = default(GameObject);

    private VoxelSpawner _vs;

    public int AmountCut;

    public string Base64MoveEncode = "";

    public Text MovesDisplay;
    public Text MpsDisplay;

    public Text TimeDisplay;

    private void Start()
    {
        _vs = FindObjectOfType<VoxelSpawner>();
    }

    private void Update()
    {
        if (_doTiming)
        {
            TimeDisplay.text = "Time: " + (Time.time - _startTime);
            MovesDisplay.text = "Moves: " + _moves;
            MpsDisplay.text =
                "MPS: " + _moves / (Time.time - _startTime);
        }

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Voxel")
            {
                _newPos = hit.transform.position;
                _newNorm = hit.normal;
                _highlightedVoxel = hit.transform.localPosition;
                _voxelHighlighter.SetActive(true);
            }
            else
            {
                _voxelHighlighter.SetActive(false);
            }
        }
        else
        {
            _voxelHighlighter.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
            _alternateSliceDir = !_alternateSliceDir;

        if (_voxelHighlighter.activeSelf && Input.GetMouseButtonDown(0))
        {
            _cubeSize = _vs.CubeSize;
            for (var c = 0; c < transform.childCount; c++)
                if (transform.GetChild(c).tag == "Moved")
                {
                    Debug.Log("BREAK AVERTED");
                    return;
                }

            if (_doTiming == false)
            {
                Base64MoveEncode = Base64Encode((int) _cubeSize);
                _timerButtonText.text = "Stop Timer";
                _startTime = Time.time;
                _doTiming = true;
                _moves = 0;
            }

            var localNorm = transform.InverseTransformDirection(_newNorm);
            _moves += 1;

            var tempReturn = true;
            for (var d = 0; d < _directionArray.Length; d++)
                if (-localNorm == _directionArray[d])
                    tempReturn = false;

            if (tempReturn)
            {
                Debug.Log("WEIRD DIRECTION: " + -localNorm);
                return;
            }

            if (!_sliceToggle.isOn)
                PushRow(_highlightedVoxel, -localNorm, true);
            else
                PushSlice(_highlightedVoxel, -localNorm);

            //Base64MoveEncoder
            Base64MoveEncode += Base64Encode((int) _highlightedVoxel.x) +
                                Base64Encode((int) _highlightedVoxel.y) +
                                Base64Encode((int) _highlightedVoxel.z);
            for (var i = 0; i < _directionArray.Length; i++)
                if (_directionArray[i] == -localNorm)
                    Base64MoveEncode += Base64Encode(i);


            var stopTimer = true;
            for (var xi = 0; xi < _cubeSize; xi++)
            for (var yi = 0; yi < _cubeSize; yi++)
            for (var zi = 0; zi < _cubeSize; zi++)
                if (_vs.VoxelArray[xi, yi, zi].Number !=
                    _vs.CorrectNumberArray[xi, yi, zi])
                    stopTimer = false;

            if (stopTimer)
            {
                Restart();
                _timerButtonText.text = "Cube Solved!";
            }
        }

        //Hardcoded values uwu
        if (!_sliceToggle.isOn)
        {
            _voxelHighlighter.transform.position = Vector3.Lerp(
                _voxelHighlighter.transform.position,
                _newPos,
                Time.deltaTime * 30
            );
            _voxelHighlighter.transform.up = Vector3.Lerp(
                _voxelHighlighter.transform.up,
                _newNorm,
                Time.deltaTime * 50
            );
        }
        else
        {
            _voxelHighlighter.transform.position = Vector3.Lerp(
                _voxelHighlighter.transform.position,
                _newPos,
                Time.deltaTime * 30
            );
            //VoxelHighlighter.transform.up = Vector3.Lerp(VoxelHighlighter.transform.up, newNorm, Time.deltaTime * 50);

            var localNorm = transform.InverseTransformDirection(_newNorm);
            var highlightUp = Vector3.zero;
            var highlightRight = Vector3.zero;
            if (Mathf.RoundToInt(Vec3Abs(localNorm).x) == 1)
            {
                highlightUp = new Vector3(0, 1, 0);
                highlightRight = new Vector3(0, 0, 1);
            }
            else if (Mathf.RoundToInt(Vec3Abs(localNorm).y) == 1)
            {
                highlightUp = new Vector3(0, 0, 1);
                highlightRight = new Vector3(1, 0, 0);
            }
            else if (Mathf.RoundToInt(Vec3Abs(localNorm).z) == 1)
            {
                highlightUp = new Vector3(0, 1, 0);
                highlightRight = new Vector3(1, 0, 0);
            }

            if (!_alternateSliceDir)
                _voxelHighlighter.transform.rotation = Quaternion.LookRotation(
                    Vector3.Lerp(
                        _voxelHighlighter.transform.forward,
                        transform.TransformDirection(highlightRight),
                        Time.deltaTime * 50
                    ),
                    Vector3.Lerp(
                        _voxelHighlighter.transform.up,
                        _newNorm,
                        Time.deltaTime * 50
                    )
                );
            else
                _voxelHighlighter.transform.rotation = Quaternion.LookRotation(
                    Vector3.Lerp(
                        _voxelHighlighter.transform.forward,
                        transform.TransformDirection(highlightUp),
                        Time.deltaTime * 50
                    ),
                    Vector3.Lerp(
                        _voxelHighlighter.transform.up,
                        _newNorm,
                        Time.deltaTime * 50
                    )
                );
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
        _voxelHighlighter.transform.Find("SliceMarkers")
            .gameObject.SetActive(_sliceToggle.isOn);
    }

    public void CutForward(Vector3 dir)
    {
        _cubeSize = _vs.CubeSize;
        if (AmountCut == (int) _cubeSize - 1) return;

        var objectsToDeactivate =
            new List<SetVoxelProperties>();
        for (var xi = 0; xi < _cubeSize; xi++)
        for (var yi = 0; yi < _cubeSize; yi++)
        for (var zi = 0; zi < _cubeSize; zi++)
        {
            SetVoxelProperties beforeVoxel;
            try
            {
                beforeVoxel = _vs.VoxelArray[
                    xi - Mathf.RoundToInt(dir.x),
                    yi - Mathf.RoundToInt(dir.y),
                    zi - Mathf.RoundToInt(dir.z)];
            }
            catch
            {
                objectsToDeactivate.Add(_vs.VoxelArray[xi, yi, zi]);
                continue;
            }

            if (beforeVoxel.Transparent)
                objectsToDeactivate.Add(_vs.VoxelArray[xi, yi, zi]);
        }

        objectsToDeactivate.ForEach(
            obj =>
            {
                obj.transform.parent.GetComponent<Collider>().enabled = false;
                obj.Transparent = true;
                obj.Redraw();
            }
        );

        AmountCut += 1;
    }

    public void CutBackward(Vector3 dir, bool resetNodulesOnZero)
    {
        if (AmountCut == 0) return;


        var objectsToActivate = new List<SetVoxelProperties>();
        for (var xi = 0; xi < _cubeSize; xi++)
        for (var yi = 0; yi < _cubeSize; yi++)
        for (var zi = 0; zi < _cubeSize; zi++)
        {
            SetVoxelProperties beforeVoxel = null;
            try
            {
                beforeVoxel = _vs.VoxelArray[xi + Mathf.RoundToInt(dir.x),
                    yi + Mathf.RoundToInt(dir.y),
                    zi + Mathf.RoundToInt(dir.z)];
            }
            catch
            {
                continue;
            }

            if (!beforeVoxel.Transparent)
                objectsToActivate.Add(_vs.VoxelArray[xi, yi, zi]);
        }

        objectsToActivate.ForEach(
            obj =>
            {
                obj.transform.parent.GetComponent<Collider>().enabled = true;
                obj.Transparent = false;
                obj.Redraw();
            }
        );

        AmountCut -= 1;

        if (AmountCut == 0 && resetNodulesOnZero)
            GetComponent<VoxelSpawner>()
                .Nodules.ForEach(n => { n.SetActive(true); });
    }

    private void PushRow(Vector3 touched, Vector3 dir, bool fancy)
    {
        _cubeSize = _vs.CubeSize;

        for (var i = 0; i < _cubeSize + 1; i++)
            if (FindVoxelByPos(touched - dir * i) == null)
            {
                touched = touched - dir * (i - 1);
                break;
            }

        var tempNumArray = new List<int>();
        for (var i = 0; i < _cubeSize; i++)
        {
            var pos = touched + dir * i;
            tempNumArray.Add(
                _vs.VoxelArray[Mathf.RoundToInt(pos.x),
                        Mathf.RoundToInt(pos.y),
                        Mathf.RoundToInt(pos.z)]
                    .Number
            );
            //Debug.Log(tempNumArray[tempNumArray.Count - 1]);
        }

        for (var i = 0; i < _cubeSize; i++)
        {
            var pos = touched + dir * i;
            if (i == 0)
                _vs.VoxelArray[Mathf.RoundToInt(pos.x),
                        Mathf.RoundToInt(pos.y),
                        Mathf.RoundToInt(pos.z)]
                    .SetNumber(tempNumArray[tempNumArray.Count - 1], fancy);
            else
                _vs.VoxelArray[Mathf.RoundToInt(pos.x),
                        Mathf.RoundToInt(pos.y),
                        Mathf.RoundToInt(pos.z)]
                    .SetNumber(tempNumArray[i - 1], fancy);
        }

        for (var c = 0; c < transform.childCount; c++)
            if (transform.GetChild(c).tag == "Moved")
                transform.GetChild(c).tag = "Voxel";
    }

    private void PushSlice(Vector3 touched, Vector3 dir)
    {
        //HACKY
        var highlightUp = Vector3.zero;
        var highlightRight = Vector3.zero;
        if (Mathf.RoundToInt(Vec3Abs(dir).x) == 1)
        {
            highlightUp = new Vector3(0, 1, 0);
            highlightRight = new Vector3(0, 0, 1);
        }
        else if (Mathf.RoundToInt(Vec3Abs(dir).y) == 1)
        {
            highlightUp = new Vector3(0, 0, 1);
            highlightRight = new Vector3(1, 0, 0);
        }
        else if (Mathf.RoundToInt(Vec3Abs(dir).z) == 1)
        {
            highlightUp = new Vector3(0, 1, 0);
            highlightRight = new Vector3(1, 0, 0);
        }

        var sliceDir = Vector3.zero;

        if (!_alternateSliceDir)
            sliceDir = highlightUp;
        else
            sliceDir = highlightRight;

        _cubeSize = _vs.CubeSize;

        for (var i = 0; i < _cubeSize + 1; i++)
            if (FindVoxelByPos(_highlightedVoxel - sliceDir * i) == null)
            {
                _highlightedVoxel = _highlightedVoxel - sliceDir * (i - 1);
                break;
            }

        for (var i = 0; i < _cubeSize; i++)
            PushRow(_highlightedVoxel + sliceDir * i, dir, true);
    }

    private Vector3 FloorVec3ToInt(Vector3 inVec)
    {
        return new Vector3(
            Mathf.FloorToInt(inVec.x),
            Mathf.FloorToInt(inVec.y),
            Mathf.FloorToInt(inVec.z)
        );
    }

    private Vector3 Vec3Abs(Vector3 inVec)
    {
        return new Vector3(
            Mathf.Abs(inVec.x),
            Mathf.Abs(inVec.y),
            Mathf.Abs(inVec.z)
        );
    }

    private Transform FindVoxelByPos(Vector3 pos)
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
            return _vs.VoxelArray[Mathf.RoundToInt(pos.x),
                    Mathf.RoundToInt(pos.y),
                    Mathf.RoundToInt(pos.z)]
                .transform;
        }
        catch
        {
            return null;
        }
    }

    public void Scramble()
    {
        if (AmountCut != 0) return;

        for (var i = 0; i < 1000; i++) ScrambleOne(true);
    }

    private void ScrambleOne(bool end)
    {
        var randVoxel = Random.Range(0, transform.childCount);
        if (transform.GetChild(randVoxel).tag == "Voxel")
            PushRow(
                transform.GetChild(randVoxel).localPosition,
                _directionArray[Random.Range(
                    0,
                    _directionArray.Length - 1
                )],
                end
            );
    }

    public string Base64Encode(int num)
    {
        var base64Table =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        return base64Table[num].ToString();
    }
}