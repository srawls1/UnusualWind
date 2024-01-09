using UnityEngine;
using UnityEngine.Events;

public class PauseListener : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private GameObject pauseScreen;
	[SerializeField] private UnityEvent<bool> m_togglePauseEvent;

	#endregion // Editor Fields

	#region Properties

	public UnityEvent<bool> togglePauseEvent
	{
		get { return m_togglePauseEvent; }
	}

	// Stop all physics (maybe set time scale to 0?)

	private bool m_paused;
	public bool paused
	{
		get { return m_paused; }
		set
		{
			m_paused = value;

			togglePauseEvent.Invoke(paused);
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		togglePauseEvent.AddListener(SetPauseScreenShowing);
		togglePauseEvent.AddListener(SetInputEnabled);
		togglePauseEvent.AddListener(SetAudioPaused);
		togglePauseEvent.AddListener(SetTimeScale);
	}

	private void Update()
	{
        if (Input.GetButtonDown("Pause"))
        {
			paused = !paused;
		}
	}

	private void OnDisable()
	{
		paused = false;
	}

	#endregion // Unity Functions

	#region Private Functions

	private void SetPauseScreenShowing(bool paused)
	{
		if (pauseScreen)
		{
			pauseScreen.SetActive(paused);
		}
	}

	private void SetInputEnabled(bool paused)
	{
		bool inputEnabled = !paused;
	}

	private void SetAudioPaused(bool paused)
	{
		AudioListener.pause = paused;
	}

	private void SetTimeScale(bool paused)
	{
		Time.timeScale = paused ? 0f : 1f;
	}

	#endregion // Private Functions
}