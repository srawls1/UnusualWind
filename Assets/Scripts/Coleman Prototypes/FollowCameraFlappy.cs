using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FollowCameraFlappy : MonoBehaviour
{
    public Transform target1; // The first object to keep in frame
    public Transform target2; // The second object to keep in frame

    public float minDistance = 5.0f; // The minimum desired distance between the camera and the objects
    public float maxDistance = 10.0f; // The maximum desired distance between the camera and the objects
    public float zoomSpeed = 5.0f; // The speed of zooming

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        // Calculate the midpoint between the two targets
        Vector3 midpoint = (target1.position + target2.position) / 2f;

        // Smoothly move the camera towards the target position
        transform.position = new Vector3(midpoint.x, midpoint.y+0.1f, transform.position.z);

        cam.orthographicSize = target1.position.y;
    }
}