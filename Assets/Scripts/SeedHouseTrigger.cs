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
    [SerializeField] private float duration;
    [SerializeField] private AnimationCurve endPositionCurve;
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
        for (int i = 0; i < letterboxes.Length; i++)
        {
            StartCoroutine(FadeInLetterbox(letterboxes[i]));

		}
		StartCoroutine(LerpPosition(impactPoint.transform.position));
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

    IEnumerator LerpPosition(Vector3 targetPosition)
    {
        float startTime = Time.time;
        yield return seedInWind.ReturnToEquilibrium();
        float equilibriumTime = Time.time;
        float timePassed = equilibriumTime - startTime;

		Vector3 startPosition = transform.position;
        float remainingXDistance = targetPosition.x - startPosition.x;
        float timeRemaining = remainingXDistance / rig.velocity.x * 0.75f;

		rotationController.enabled = false;
		seedInWind.enabled = false;
		Vector2 velocity = rig.velocity;
		rig.isKinematic = true;
		GetComponent<Collider2D>().enabled = false;


		for (float time = 0f; time < timeRemaining; time += Time.fixedDeltaTime)
        {
            //transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, timeRemaining - time);
            float t = endPositionCurve.Evaluate(time / timeRemaining);
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return new WaitForFixedUpdate();
        }

		mainAnimator.SetBool("Resting", true);
        transform.position = targetPosition;

        houseAnimation.SetActive(true);
        gameObject.SetActive(false);
        transform.SetParent(houseAnimation.transform, false);
        transform.localPosition = Vector3.zero;
	}
}