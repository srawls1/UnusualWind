using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float minimumY;
	[SerializeField] private float maximumY;
	[SerializeField] private float paddingAboveTarget;
	[SerializeField] private float minimumX;
	[SerializeField] private float maximumX;
	[SerializeField] private float minimumSize;
	[SerializeField] private float positionDamping;
	[SerializeField] private float sizeDamping;
	[SerializeField] private float lookaheadTime;
	[SerializeField] private float xOffset;

	new private Camera camera;
	private Vector3 previousPosition;
	private Vector3 runningAverageVelocity;
	private Vector3 unclampedPosition;

	private void Awake()
	{
		camera = transform.GetComponent<Camera>();

		//float bottom = minimumY;
		//float top = followTarget.position.y + paddingAboveTarget;
		//float height = top - bottom;
		//height = Mathf.Max(height, minimumSize);
		//float width = camera.aspect * height;
		//float x = Mathf.Clamp(followTarget.position.x, minimumX + width / 2, maximumX - width / 2);
		//float y = Mathf.Clamp((bottom + top) * .5f, minimumY + height / 2, maximumY - height / 2);
		//transform.position = new Vector3(x + xOffset, y, transform.position.z);
		//camera.orthographicSize = height * 0.5f;
		previousPosition = followTarget.position;
	}

	private void FixedUpdate()
	{
		Vector3 diff = followTarget.position - previousPosition;
		Vector3 velocityLastFrame = diff / Time.deltaTime;
		runningAverageVelocity *= 0.9f;
		runningAverageVelocity += velocityLastFrame * 0.1f;
		previousPosition = followTarget.position;
		Vector3 forecastedPosition = followTarget.position + runningAverageVelocity * lookaheadTime;

		float bottom = minimumY;
		float top = forecastedPosition.y + paddingAboveTarget;
		float height = top - bottom;
		height = Mathf.Max(height, minimumSize) * 0.5f;
		float width = camera.aspect * height;
		
		unclampedPosition = new Vector3(forecastedPosition.x + xOffset, (bottom + top) * 0.5f, transform.position.z);
		unclampedPosition = Vector3.Lerp(transform.position, unclampedPosition, positionDamping * Time.deltaTime);

		float nextSize = Mathf.Lerp(camera.orthographicSize, height, sizeDamping * Time.deltaTime);
		camera.orthographicSize = nextSize;

		Vector3 nextPosition = new Vector3(
			Mathf.Clamp(unclampedPosition.x, minimumX + width, maximumX - width),
			Mathf.Clamp(unclampedPosition.y, minimumY + height, maximumY - height),
			transform.position.z
		);

		transform.position = nextPosition;
	}

	private void OnDrawGizmosSelected()
	{
		float centerX = (minimumX + maximumX) * 0.5f;
		float centerY = (minimumY + maximumY) * 0.5f;
		float width = maximumX - minimumX;
		float height = maximumY - minimumY;
		Gizmos.DrawWireCube(new Vector3(centerX, centerY), new Vector3(width, height));
	}
}
