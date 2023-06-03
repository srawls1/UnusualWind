using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [SerializeField] private Transform referencePoint;
    [SerializeField] private float relativePositionChange;
    [SerializeField] private float relativeRotationChange;

    private Vector3 lastReferencePosition;
    private Vector3 lastReferenceEulerAngles;

	private void Awake()
	{
        lastReferencePosition = referencePoint.position;
        lastReferenceEulerAngles = referencePoint.eulerAngles;
	}

	private void LateUpdate()
	{
		Vector3 referencePosition = referencePoint.position;
		Vector3 positionDiff = referencePosition - lastReferencePosition;
		transform.position += positionDiff * relativePositionChange;
		lastReferencePosition = referencePosition;

		Vector3 referenceEulerAngles = referencePoint.eulerAngles;
		Vector3 eulerAngleDiff = referenceEulerAngles - lastReferenceEulerAngles;
		transform.eulerAngles += eulerAngleDiff * relativeRotationChange;
		lastReferenceEulerAngles = referenceEulerAngles;
	}
}
