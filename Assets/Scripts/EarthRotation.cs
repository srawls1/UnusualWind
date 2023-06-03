using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    [SerializeField] private Transform referencePoint;
    [SerializeField] private float radius;

    private float radius2deg;

    void Awake()
    {
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
