using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float minimumY;
	[SerializeField] private float maximumY;
	[SerializeField, FormerlySerializedAs("paddingAboveTarget")] private float paddingAroundTarget;
	[SerializeField] private float minimumX;
	[SerializeField] private float maximumX;
	[SerializeField] private float minimumSize;
	[SerializeField] private float positionDamping;
	[SerializeField] private float sizeDamping;
	[SerializeField] private float lookaheadTime;
	[SerializeField] private float rightEdgeMinLookaheadTime;
	[SerializeField, FormerlySerializedAs("minTopY")] private float m_minTopY;

	new private Camera camera;
	private Queue<float> previousFrameVelocities;
	private float previousPosition;
	private float runningAverageVelocity;

	public float minTopY
	{
		get { return m_minTopY; }
		set { m_minTopY = value; }
	}

	public float currentTopY
	{
		get { return transform.position.y + camera.orthographicSize; }
	}

	private void Awake()
	{
		camera = transform.GetComponent<Camera>();
		previousFrameVelocities = new Queue<float>();

		//float bottom = minimumY;
		//float top = followTarget.position.y + paddingAboveTarget;
		//float height = top - bottom;
		//height = Mathf.Max(height, minimumSize);
		//float width = camera.aspect * height;
		//float x = Mathf.Clamp(followTarget.position.x, minimumX + width / 2, maximumX - width / 2);
		//float y = Mathf.Clamp((bottom + top) * .5f, minimumY + height / 2, maximumY - height / 2);
		//transform.position = new Vector3(x + xOffset, y, transform.position.z);
		//camera.orthographicSize = height * 0.5f;
		previousPosition = followTarget.position.x;
	}

	// Desired bottom: minimumY
	// Desired top: forecastedPosition.y + padding, clamped
	// Desired left: min(forecastedPosition.x - screenWidth, followPosition.x - padding)
	// Desired right: max(forecastedPosition.x + screenWidth, followPosition.x + padding, rightEdgeMinLookahead)
	// Calculate desired width and height
	// If it's too wide:
	// - Raise the top accordingly
	// - If the top gets too high:
	// - - we'll have to begrudgingly move the right edge in
	// If it's too tall:
	// - Move left and right out equally


	private void FixedUpdate()
	{
		float diff = followTarget.position.x - previousPosition;
		float velocityLastFrame = diff / Time.deltaTime;
		runningAverageVelocity *= 50;
		runningAverageVelocity += velocityLastFrame;
		previousFrameVelocities.Enqueue(velocityLastFrame);

		if (previousFrameVelocities.Count > 50)
		{
			float dequeuedVelocity = previousFrameVelocities.Dequeue();
			runningAverageVelocity -= dequeuedVelocity;
		}

		runningAverageVelocity /= 50;
		previousPosition = followTarget.position.x;

		float velocityUsedForForecast = //previousFrameVelocities.Count >= 50 ?
			runningAverageVelocity /*: 0f*/;
		Vector3 forecastedPosition = followTarget.position + Vector3.right * velocityUsedForForecast * lookaheadTime;
		float rightEdgeForecast = followTarget.position.x + velocityUsedForForecast * rightEdgeMinLookaheadTime;

		float bottom = minimumY;
		float top = Mathf.Clamp(forecastedPosition.y + paddingAroundTarget, minTopY, maximumY);
		float left = followTarget.position.x - paddingAroundTarget;
		float right = Mathf.Max(followTarget.position.x + paddingAroundTarget, rightEdgeForecast);

		Debug.DrawLine(new Vector2(left, bottom), new Vector2(right, bottom));
		Debug.DrawLine(new Vector2(left, bottom), new Vector2(left, top));
		Debug.DrawLine(new Vector2(left, top), new Vector2(right, top));
		Debug.DrawLine(new Vector2(right, bottom), new Vector2(right, top));

		float height = top - bottom;
		float width = right - left;

		// If it's too wide
		if (width > camera.aspect * height)
		{
			height = width / camera.aspect;
			top = bottom + height;
			if (top > maximumY)
			{
				top = maximumY;
				height = top - bottom;
				width = height * camera.aspect;
				right = left + width;
			}

			Debug.DrawLine(new Vector2(left, bottom), new Vector2(right, bottom), Color.red);
			Debug.DrawLine(new Vector2(left, bottom), new Vector2(left, top), Color.red);
			Debug.DrawLine(new Vector2(left, top), new Vector2(right, top), Color.red);
			Debug.DrawLine(new Vector2(right, bottom), new Vector2(right, top), Color.red);
		}
		// If it's too tall
		else
		{
			float idealWidth = width;
			width = height * camera.aspect;
			float widthReduction = idealWidth - width;
			left += widthReduction * 0.5f;
			right -= widthReduction * 0.5f;

			Debug.DrawLine(new Vector2(left, bottom), new Vector2(right, bottom), Color.blue);
			Debug.DrawLine(new Vector2(left, bottom), new Vector2(left, top), Color.blue);
			Debug.DrawLine(new Vector2(left, top), new Vector2(right, top), Color.blue);
			Debug.DrawLine(new Vector2(right, bottom), new Vector2(right, top), Color.blue);
		}

		width *= 0.5f;
		height *= 0.5f;

		Vector3 unclampedPosition = new Vector3((left + right) * 0.5f, (top + bottom) * 0.5f, transform.position.z);
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
		Gizmos.color = Color.green;
		centerY = (minimumY + minTopY) * 0.5f;
		height = minTopY - minimumY;
		Gizmos.DrawWireCube(new Vector3(centerX, centerY), new Vector3(width, height));
	}
}
