using UnityEngine;

public class ChangeColorOnHover : MonoBehaviour {

    private Color _startColor;
    public Color ToColor;
    private Color _lerpToColor;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
        _startColor = GetComponent<MeshRenderer>().material.GetColor("_Color");
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                _lerpToColor = ToColor;
            }
            else
            {
                _lerpToColor = _startColor;
            }
        } else
        {
            _lerpToColor = _startColor;
        }

        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.Lerp(GetComponent<MeshRenderer>().material.color, _lerpToColor, Time.deltaTime * 15));
    }
}
