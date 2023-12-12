using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
	[SerializeField] new private Camera camera;
    [SerializeField] private float relativePositionChange;
	private float areaDistance = 1000f;

	[SerializeField] private bool shouldWait;
	[SerializeField] private float distanceToPlayer;

	private Vector3 previousCameraPosition;

	private void Start()
	{
		previousCameraPosition = camera.transform.position;
	}

	private void Update()
	{
		if (shouldWait && Vector3.Distance(transform.position, camera.transform.position) > distanceToPlayer)
		{
            return;
        }
        Vector3 diff = camera.transform.position - previousCameraPosition;
        previousCameraPosition = camera.transform.position;
        diff = Vector3.Scale(diff, Vector3.right);
        transform.Translate(diff * relativePositionChange);
    }
}
