using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    [SerializeField] private HubRotatedChannelSO rotationChannel;
	[SerializeField] private float offset;

	private void OnEnable()
	{
		rotationChannel.Subscribe(FollowRotation);
	}

	private void OnDisable()
	{
		rotationChannel.Unsubscribe(FollowRotation);
	}

	private void FollowRotation(float currentRotation, float rotationDelta)
	{
		transform.eulerAngles = new Vector3(0f, 0f, currentRotation + offset);
	}
}
