using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The actor that the camera should follow
    public float smoothSpeed = 0.125f; // The speed at which the camera should follow the actor
    public Vector3 offset; // The offset from the actor's position that the camera should be

    public void Init(Transform target)
    {
        this.target = target;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the camera's position based on the actor's position and the specified offset
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.y = transform.position.y; // Keep the camera's y position fixed

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
