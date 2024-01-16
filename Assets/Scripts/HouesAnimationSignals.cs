using UnityEngine;
using UnityEngine.Events;

public class HouesAnimationSignals : MonoBehaviour
{
	[SerializeField] private UnityEvent fadeOutMusicEvent, stopWindEvent, setEndCameraEvent, loadLoopSceneEvent;


	public void SendMusicStopSignal()
	{
		fadeOutMusicEvent.Invoke();
	}

	public void SendWindStopSignal()
	{
		stopWindEvent.Invoke();
	}

	public void SendEndCameraSignal()
	{
		setEndCameraEvent.Invoke();
	}

	public void SendLoopSceneSignal()
	{
		loadLoopSceneEvent.Invoke();
	}
}
