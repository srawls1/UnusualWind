using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
	[SerializeField] private HubRotatedChannelSO rotationChannel;
    [SerializeField] private float relativeRotationChange;

	private void OnEnable()
	{
		rotationChannel.Subscribe(UpdateRotation);
	}

	private void OnDisable()
	{
		rotationChannel.Unsubscribe(UpdateRotation);
	}

	private void UpdateRotation(float currentRotation, float rotationDelta)
	{
		transform.eulerAngles += new Vector3(0f, 0f, rotationDelta * relativeRotationChange);
	}
}
