using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SeedHouseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject impactPoint;
    private SeedInWind seedInWind;
    private RotationController rotationController;
    private Animator mainAnimator;
    private Rigidbody2D rig;
    [SerializeField] private float duration = 5f;
    [SerializeField] private GameObject houseAnimation;
    [SerializeField] private Image[] letterboxes;
    [SerializeField] private float letterboxFadeDuration;

	private void Start()
    {
        seedInWind = GetComponent<SeedInWind>();
        if (seedInWind == null)
        {
            Debug.LogError("Seed in wind script not found");
        }
        rotationController = GetComponent<RotationController>();
        if (rotationController == null)
        {
            Debug.LogError("Rotation controller script not found");
        }
        mainAnimator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    public void HouseStart()
    {
        rotationController.enabled = false;
        seedInWind.enabled = false;
		rig.isKinematic = true;
		GetComponent<Collider2D>().enabled = false;

		mainAnimator.SetBool("Resting", true);

        for (int i = 0; i < letterboxes.Length; i++)
        {
            StartCoroutine(FadeInLetterbox(letterboxes[i]));

		}
		StartCoroutine(LerpPosition(impactPoint.transform.position, duration));
    }

	private IEnumerator FadeInLetterbox(Image letterbox)
	{
        Color startColor = Color.clear;
        Color endColor = Color.black;
        letterbox.gameObject.SetActive(true);

        for (float dt = 0f; dt < letterboxFadeDuration; dt += Time.deltaTime)
        {
            letterbox.color = Color.Lerp(startColor, endColor, dt / letterboxFadeDuration);
            yield return null;
        }

        letterbox.color = endColor;
	}

	void OnTriggerEnter2D(Collider2D collider)
    {
        //If collision is called house, call house functionz
        if (collider.gameObject.name == "HouseTrigger")
        {
            HouseStart();
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            Vector2 velocity = rig.velocity;
            float yVel = velocity.y;
            float finalYPos = targetPosition.y;
            float finalXPos = targetPosition.x;
            transform.position = new Vector2(Mathf.Lerp(startPosition.x, finalXPos, time/duration), Mathf.SmoothDamp(transform.position.y, finalYPos, ref yVel, duration - time));
            velocity.y = yVel;
            rig.velocity = velocity;
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = targetPosition;

        houseAnimation.SetActive(true);
        gameObject.SetActive(false);
        transform.SetParent(houseAnimation.transform, false);
        transform.localPosition = Vector3.zero;
	}
}