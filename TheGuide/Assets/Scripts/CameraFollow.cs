using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // public variables
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 setPoint = Vector3.zero;

    // private variables
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 _targetPosition = target.TransformPoint(setPoint);
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref velocity, smoothTime);
        float _y = transform.localEulerAngles.y;
        transform.LookAt(target);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _y, transform.localEulerAngles.z);
    }
}
