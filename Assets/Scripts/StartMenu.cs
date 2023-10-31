using System.Collections;
using UnityEngine;
using Rewired;
using TMPro;

public class StartMenu : MonoBehaviour
{
	[SerializeField] private Rigidbody2D seedRigidBody;
	[SerializeField] private CameraMovement cameraMovement;
	[SerializeField] private GlowingText menuTextGlow;
	[SerializeField] private TextMeshProUGUI menuText;
	[SerializeField] private float textDisappearDuration;

	private Player player;

	private void Awake()
	{
		player = ReInput.players.GetPlayer(0);
	}

	private void Update()
	{
		if (player.GetAnyButtonDown())
		{
			StartCoroutine(StopTextGlowing());
		}
		if (player.GetAnyButtonUp())
		{
			StartCoroutine(DisableMenu());
		}
	}

	private IEnumerator DisableMenu()
	{
		seedRigidBody.bodyType = RigidbodyType2D.Dynamic;
		cameraMovement.enabled = true;

		Color startTextColor = menuText.color;
		Color endTextColor = startTextColor;
		endTextColor.a = 0;

		for (float dt = 0f; dt < textDisappearDuration; dt += Time.deltaTime)
		{
			menuText.color = Color.Lerp(startTextColor, endTextColor, dt / textDisappearDuration);
			yield return null;
		}

		menuText.color = endTextColor;
		gameObject.SetActive(false);
	}

	private IEnumerator StopTextGlowing()
	{
		float startMin = menuTextGlow.minGlowPower;
		float startMax = menuTextGlow.maxGlowPower;
		
		for (float dt = 0f; dt < textDisappearDuration; dt += Time.deltaTime)
		{
			menuTextGlow.minGlowPower = Mathf.Lerp(startMin, 0, dt / textDisappearDuration);
			menuTextGlow.maxGlowPower = Mathf.Lerp(startMax, 0, dt / textDisappearDuration);
			yield return null;
		}

		menuTextGlow.minGlowPower = 0;
		menuTextGlow.maxGlowPower = 0;
	}
}
