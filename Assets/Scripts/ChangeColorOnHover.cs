using UnityEngine;

public class ChangeColorOnHover : MonoBehaviour
{
    private Color _lerpToColor;
    private Color _startColor;
    public Color ToColor;

    // Use this for initialization
    private void Start()
    {
        GetComponent<MeshRenderer>().material =
            new Material(GetComponent<MeshRenderer>().material);
        _startColor = GetComponent<MeshRenderer>().material.GetColor("_Color");
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            var hitId = hit.transform.gameObject.GetInstanceID();
            _lerpToColor = hitId == gameObject.GetInstanceID()
                ? ToColor
                : _startColor;
        }
        else
        {
            _lerpToColor = _startColor;
        }

        GetComponent<MeshRenderer>()
            .material.SetColor(
                "_Color",
                Color.Lerp(
                    GetComponent<MeshRenderer>().material.color,
                    _lerpToColor,
                    Time.deltaTime * 15
                )
            );
    }
}