using System.Collections;
using UnityEngine;

public class SecondLoopStart : MonoBehaviour
{
	[SerializeField] private Rigidbody2D seedRigidBody;
	[SerializeField] private SeedInWind seed;
	[SerializeField] private RotationController rotationController;
	[SerializeField] private CameraMovement cameraMovement;
	[SerializeField] private Animator cameraAnimator;
	[SerializeField] private float cameraExpansionTime = 0.3f;
	[SerializeField] private bool fart;

	public void StartPlayerMoving()
	{
		StartCoroutine(DisableMenu());
	}

	private IEnumerator DisableMenu()
	{
		seedRigidBody.bodyType = RigidbodyType2D.Dynamic;
		seed.enabled = true;
		rotationController.startScene = true;
		cameraMovement.enabled = true;
		cameraAnimator.enabled = false;

		yield return cameraMovement.StartCoroutine(ExpandMinTopY());
	}

	private IEnumerator ExpandMinTopY()
	{
		float startingMinTopY = cameraMovement.currentTopY;
		float endingMinTopY = cameraMovement.minTopY;
		cameraMovement.minTopY = startingMinTopY;

		for (float dt = 0f; dt < cameraExpansionTime; dt += Time.deltaTime)
		{
			cameraMovement.minTopY = Mathf.Lerp(startingMinTopY, endingMinTopY, dt / cameraExpansionTime);
			yield return null;
		}

		cameraMovement.minTopY = endingMinTopY;
	}
}
