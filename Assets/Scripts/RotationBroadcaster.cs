using UnityEngine;

public class RotationBroadcaster : MonoBehaviour
{
    [SerializeField] private HubRotatedChannelSO broadcastChannel;

	private float previousRotation;

	private void Awake()
	{
		previousRotation = transform.eulerAngles.z;
	}

	private void LateUpdate()
	{
		float rotation = transform.eulerAngles.z;
		float deltaRotation = rotation - previousRotation;
		if (deltaRotation > 180)
		{
			deltaRotation -= 360;
		}
		if (deltaRotation < -180)
		{
			deltaRotation += 360;
		}
		broadcastChannel.Broadcast(rotation, deltaRotation);
		previousRotation = rotation;
	}
}
