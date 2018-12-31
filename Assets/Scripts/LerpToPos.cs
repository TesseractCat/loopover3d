using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPos : MonoBehaviour {

    bool lerping = false;
    Vector3 newPos;
    float lerpSpeed;

    void Update()
    {
        if (lerping)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * lerpSpeed);
        }
        if (Mathf.Abs(transform.localPosition.magnitude - newPos.magnitude) < 0.1f)
        {
            lerping = false;
            transform.localPosition = newPos;
        }
    }

    public void MoveToPos(Vector3 pos, float speed)
    {
        lerpSpeed = speed;
        newPos = pos;
        lerping = true;
    }

}
