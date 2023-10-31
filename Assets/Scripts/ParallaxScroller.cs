using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
	[SerializeField] new private Camera camera;
    [SerializeField] private float relativePositionChange;

	private Vector3 previousCameraPosition;

	private void Start()
	{
		previousCameraPosition = camera.transform.position;
	}

	private void Update()
	{
		Vector3 diff = camera.transform.position - previousCameraPosition;
		previousCameraPosition = camera.transform.position;
		diff = Vector3.Scale(diff, Vector3.right);
		transform.Translate(diff * relativePositionChange);
	}
}
