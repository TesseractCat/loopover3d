using UnityEngine;

public class LerpToPos : MonoBehaviour
{
    private bool _lerping;
    private float _lerpSpeed;
    private Vector3 _newPos;

    private void Update()
    {
        if (_lerping)
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                _newPos,
                Time.deltaTime * _lerpSpeed
            );

        if (Mathf.Abs(transform.localPosition.magnitude - _newPos.magnitude) <
            0.1f)
        {
            _lerping = false;
            transform.localPosition = _newPos;
        }
    }

    public void MoveToPos(Vector3 pos, float speed)
    {
        _lerpSpeed = speed;
        _newPos = pos;
        _lerping = true;
    }
}