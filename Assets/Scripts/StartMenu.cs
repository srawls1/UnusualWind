using System.Collections;
using UnityEngine;
using Rewired;
using TMPro;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
	[SerializeField] private Rigidbody2D seedRigidBody;
	[SerializeField] private SeedInWind seed;
	[SerializeField] private CameraMovement cameraMovement;
	[SerializeField] private GlowingText menuTextGlow;
	[SerializeField] private TextMeshProUGUI menuText;
	[SerializeField] private Image titleImage;
	[SerializeField] private float textDisappearDuration;
	[SerializeField] private float timeToStart;
	[SerializeField] private bool fart;

	private bool previouslyUnderTime;
    private float timeHeld;

    private Player player;

	private void Awake()
	{
		player = ReInput.players.GetPlayer(0);
	}

	private void Update()
	{
		if (timeHeld >= timeToStart)
		{
			if (previouslyUnderTime)
			{
				StartCoroutine(AdvanceText());
			}

			if (player.GetAnyButtonUp())
            {
                StartCoroutine(DisableMenu());
            }
        }

		previouslyUnderTime = timeHeld < timeToStart;
		if (player.GetAnyButton())
		{
			timeHeld += Time.deltaTime;
		}
		else { timeHeld = 0; }
    }

	private IEnumerator AdvanceText()
	{
		yield return StartCoroutine(FadeOutText());
		menuText.text = "Release";
		yield return StartCoroutine(FadeInText());
	}

	private IEnumerator FadeOutText()
	{
		Color startTextColor = menuText.color;
		Color endTextColor = startTextColor;
		endTextColor.a = 0;

		return FadeText(startTextColor, endTextColor);
	}

	private IEnumerator FadeInText()
	{
		Color startTextColor = menuText.color;
		Color endTextColor = startTextColor;
		endTextColor.a = 1;

		return FadeText(startTextColor, endTextColor);
	}

	private IEnumerator FadeText(Color startColor, Color endColor)
	{
		for (float dt = 0f; dt < textDisappearDuration; dt += Time.deltaTime)
		{
			menuText.color = Color.Lerp(startColor, endColor, dt / textDisappearDuration);
			yield return null;
		}

		menuText.color = endColor;
	}

	private IEnumerator DisableMenu()
	{
		seedRigidBody.bodyType = RigidbodyType2D.Dynamic;
		seed.enabled = true;
		cameraMovement.enabled = true;
		yield return StartCoroutine(FadeOutText());
		
		menuText.gameObject.SetActive(false);
	}
}
