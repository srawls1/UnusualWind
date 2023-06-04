using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    [SerializeField] private string referencePointTag;
    [SerializeField] private float radius;

    private Transform referencePoint;
    private float radius2deg;

    void Awake()
    {
        referencePoint = GameObject.FindGameObjectWithTag(referencePointTag).transform;
        radius2deg = Mathf.Rad2Deg / radius;
    }

    void LateUpdate()
    {
        Vector3 referencePosition = referencePoint.position;
        float xDif = referencePosition.x - transform.position.x;
        referencePoint.position -= xDif * Vector3.right;
        float angularDif = xDif * radius2deg;
        transform.Rotate(Vector3.forward, angularDif);
    }
}
