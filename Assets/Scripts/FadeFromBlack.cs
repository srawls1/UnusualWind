using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeFromBlack : MonoBehaviour
{
	[SerializeField] private float fadeDuration;

	private IEnumerator Start()
	{
		Image image = GetComponent<Image>();

		Color startColor = Color.black;
		Color endColor = Color.clear;

		for (float dt = 0; dt < fadeDuration; dt += Time.deltaTime)
		{
			image.color = Color.Lerp(startColor, endColor, dt / fadeDuration);
			yield return null;
		}

		image.color = endColor;
		image.gameObject.SetActive(false);
	}
}
