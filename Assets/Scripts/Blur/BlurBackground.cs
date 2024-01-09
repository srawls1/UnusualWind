using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class BlurBackground : MonoBehaviour
{
	[SerializeField] private PauseListener pauseListener;
	[SerializeField] private float blurStrength = 5f;
	[SerializeField] private float lerpDuration = 0.2f;
    [SerializeField] private VolumeProfile urpVolumeProfile;

	private BlurSettings settings;

	private void OnEnable()
	{
		pauseListener.togglePauseEvent.AddListener(ToggleBlur);

		if (!urpVolumeProfile.TryGet<BlurSettings>(out settings))
		{
			Debug.LogWarning("Failed to get blur settings; disabling self");
			enabled = false;
		}
	}

	private void OnDisable()
	{
		pauseListener.togglePauseEvent.RemoveListener(ToggleBlur);
	}

	private void ToggleBlur(bool paused)
	{
		if (paused)
		{
			TurnOnBlur();
		}
		else
		{
			TurnOffBlur();
		}
	}

	public void TurnOnBlur()
	{
		Debug.Log("Turning on blur");
		StopAllCoroutines();
		StartCoroutine(LerpBlur(blurStrength, true));
	}

	public void TurnOffBlur()
	{
		Debug.Log("Turning off blur");
		StopAllCoroutines();
		StartCoroutine(LerpBlur(0, false));
		
	}

	private IEnumerator LerpBlur(float goalBlurStrength, bool enableStrength)
	{
		settings.active = true;
		float startStrength = settings.strength.value;
		float endStrength = goalBlurStrength;

		for (float dt = 0f; dt < lerpDuration; dt += Time.unscaledDeltaTime)
		{
			settings.strength.value = Mathf.Lerp(startStrength, endStrength, dt / lerpDuration);
			yield return null;
		}

		Debug.Log($"Updated blur strength to {goalBlurStrength}");
		settings.strength.value = goalBlurStrength;
		settings.active = enableStrength;
	}
}
