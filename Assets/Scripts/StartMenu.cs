using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System;

public class StartMenu : MonoBehaviour
{
	[SerializeField] private Rigidbody2D seedRigidBody;
	[SerializeField] private SeedInWind seed;
	[SerializeField] private CameraMovement cameraMovement;
	[SerializeField] private GlowingText menuTextGlow;
	[SerializeField] private TextMeshProUGUI menuText;
	[SerializeField] private TextMeshProUGUI petalText;
	[SerializeField] private Image titleImage;
	[SerializeField] private Light2D titleLight;
	[SerializeField] private float textDisappearDuration;
	[SerializeField] private float timeToStart;
	[SerializeField] private float timeToStartTutorial = .5f;
	[SerializeField] private float titleDisappearDuration;
	[SerializeField] private float cameraExpansionTime = 0.3f;
	[SerializeField] private bool fart;
	[SerializeField] private int upDownCount = 0;
	[SerializeField] private Vector3 menuTextOffset = new Vector3(0, 0, 0);
	[SerializeField] private float menuTextAlpha = 1f;
	[SerializeField] private float menuTextAlphaTutorial = 1f;
	[SerializeField] private float textAppearDelay = 1.5f;


	private bool anyKeyPreviouslyHeld;
	private bool anyKeyHeld;
	private bool anyKeyReleased;
	private bool previouslyUnderTime;
    private float timeHeld;

	private IEnumerator Start()
	{
		menuText.color = new Color(1, 1, 1, 0);
		yield return new WaitForSeconds(textAppearDelay);
		yield return StartCoroutine(FadeInText());
	}

	private void Update()
	{
		UpdateInput();
		if (timeHeld >= timeToStart)
		{
			if (previouslyUnderTime)
			{
				StartCoroutine(AdvanceTitleText());
				StartCoroutine(AdvanceText());
			}

			if (anyKeyReleased)
            {
                StartCoroutine(DisableMenu());
            }
        }
		previouslyUnderTime = timeHeld < timeToStart;
		if (anyKeyHeld)
		{
			timeHeld += Time.deltaTime;
		}
		else { timeHeld = 0; }

		if (upDownCount > 0) {
			//Gradually move menu text to the player's position plus the offset
			menuText.transform.position = Vector3.Lerp(menuText.transform.position, 
				Camera.main.WorldToScreenPoint(seedRigidBody.gameObject.transform.position) + menuTextOffset, Time.deltaTime * 5);
		} 

		if (upDownCount > 5 && menuText.alpha == 0) {
			menuText.gameObject.SetActive(false);
		}
    }

	private void UpdateInput()
	{
		bool anyKeyCurrentlyPressed = Input.anyKey && !Input.GetButton("Pause");
		anyKeyHeld = anyKeyCurrentlyPressed;
		anyKeyReleased = !anyKeyHeld && anyKeyPreviouslyHeld;
		anyKeyPreviouslyHeld = anyKeyHeld;
	}

	private IEnumerator AdvanceTitleText()
	{
		yield return StartCoroutine(FadeOutTitle());
		yield return StartCoroutine(FadeText(petalText, Color.clear, Color.white));
	}

	private IEnumerator AdvanceText()
	{
		yield return StartCoroutine(FadeOutText());
		if (menuText.alpha == 0)
		{
			menuText.text = "Release";
		}
		yield return StartCoroutine(FadeInText());
	}

	private IEnumerator RevertText()
	{
		yield return StartCoroutine(FadeOutText());
		if (menuText.alpha == 0)
		{
			menuText.text = "Hold any key";
		}
		yield return StartCoroutine(FadeInText());
	}

	private IEnumerator FadeOutTitle()
	{
		Color startColor = titleImage.color;
		Color endColor = startColor;
		endColor.a = 0;

		for (float dt = 0f; dt < titleDisappearDuration; dt += Time.deltaTime)
		{
			titleImage.color = Color.Lerp(startColor, endColor, dt / titleDisappearDuration);
			titleLight.intensity = Mathf.Lerp(1, 0, dt / titleDisappearDuration);

			yield return null;
		}

		titleImage.color = endColor;
	}

	private IEnumerator FadeOutText()
	{
		Color startTextColor = menuText.color;
		Color endTextColor = startTextColor;
		endTextColor.a = 0;

		return FadeText(menuText, startTextColor, endTextColor);
	}

	private IEnumerator FadeInText()
	{
		Color startTextColor = menuText.color;
		Color endTextColor = startTextColor;
		endTextColor.a = menuTextAlpha;

		return FadeText(menuText, startTextColor, endTextColor);
	}

	private IEnumerator FadeText(TextMeshProUGUI text, Color startColor, Color endColor)
	{
		for (float dt = 0f; dt < textDisappearDuration; dt += Time.deltaTime)
		{
			text.color = Color.Lerp(startColor, endColor, dt / textDisappearDuration);
			yield return null;
		}

		text.color = endColor;
	}

	private IEnumerator DisableMenu()
	{
		timeToStart = timeToStartTutorial;
		menuTextAlpha = menuTextAlphaTutorial;
		menuTextAlphaTutorial -= .05f;
		CoroutineStopper();


		if (upDownCount == 0) {
			seedRigidBody.bodyType = RigidbodyType2D.Dynamic;
			seed.enabled = true;
			upDownCount++;

			Coroutine expandCoroutine = cameraMovement.StartCoroutine(ExpandMinTopY());
			Coroutine revertTextCoroutine = StartCoroutine(RevertText());

			yield return expandCoroutine;
			yield return revertTextCoroutine;
		}
		
		if (upDownCount <= 5) {
			yield return StartCoroutine(RevertText());
		}
		else {
			yield return StartCoroutine(FadeOutText());
		}
		
		upDownCount++;
	}

	private IEnumerator ExpandMinTopY()
	{
		float startingMinTopY = cameraMovement.currentTopY;
		float endingMinTopY = cameraMovement.minTopY;
		cameraMovement.minTopY = startingMinTopY;
		cameraMovement.enabled = true;

		for (float dt = 0f; dt < cameraExpansionTime; dt += Time.deltaTime)
		{
			cameraMovement.minTopY = Mathf.Lerp(startingMinTopY, endingMinTopY, dt / cameraExpansionTime);
			yield return null;
		}

		cameraMovement.minTopY = endingMinTopY;
	}

	void CoroutineStopper() {
		StopAllCoroutines();
		StartCoroutine(FadeOutTitle());
	}
}
