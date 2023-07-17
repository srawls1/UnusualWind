using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class HubRotatedChannelSO : ScriptableObject
{
    [SerializeField] private UnityEvent<float, float> rotationChangedEvent;

    public void Subscribe(UnityAction<float, float> rotationChangedCallback)
	{
		rotationChangedEvent.AddListener(rotationChangedCallback);
	}

	public void Unsubscribe(UnityAction<float, float> rotationChangedCallback)
	{
		rotationChangedEvent.RemoveListener(rotationChangedCallback);
	}

	public void Broadcast(float currentRotation, float rotationDelta)
	{
		rotationChangedEvent.Invoke(currentRotation, rotationDelta);
	}
}
