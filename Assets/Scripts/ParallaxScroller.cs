using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
	[SerializeField] new private Camera camera;
    [SerializeField] private float relativePositionChange;
	private float areaDistance = 1000f;

	private Vector3 startingCameraPosition;
	private Vector3 startingPosition;

	private void Start()
	{
		startingCameraPosition = camera.transform.position;
		startingPosition = transform.position;
	}

	private void Update()
	{
        Vector3 diff = camera.transform.position - startingCameraPosition;
        diff = Vector3.Scale(diff, Vector3.right);
		transform.position = startingPosition + diff * relativePositionChange;
    }
}
