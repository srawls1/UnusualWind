using System.Collections;
using UnityEngine.Serialization;
using UnityEngine;
using Rewired;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class SeedInWind : MonoBehaviour
{
	private enum SeedMovementState
	{
		Equilibrium,
		Diving,
		Rising,
		FallingToEquilibrium
	}

	[SerializeField] [FormerlySerializedAs("horizontalSpeed")] private float baseHorizontalSpeed = 4f;
	[SerializeField] private float groundAltitude;
	[SerializeField] private float equilibriumAltitude;
	[SerializeField] private float topAltitude;
	[SerializeField] private float equilibriumOscillationPeriod = 3f;
	[SerializeField] private float equilibriumOscillationAmplitude = 1f;
	[SerializeField] private float fallDuration;
	[SerializeField] private float fullReboundDuration;
	[SerializeField] private float fullReboundDistanceMultiplier = 1.2f;
	[SerializeField] private float maxFallToEquilibriumDuration;
	[SerializeField] private float fullReboundSpeedBoost = 3f;
	[SerializeField] private float horizontalSpeedDecayRate = 2f;
	[SerializeField] private float maxHorizontalSpeed = 15f;
	[SerializeField] private float maxBosstedHorizontalSpeed = 25f;
	[SerializeField] private GameObject speedBoost;

	private float normalVelocity = 15f;
	private float fallGravityScale;
	private float fullReboundDistance;
	private float equilibriumSpringCoefficient;
	private SeedMovementState state;

	private Player player;
	new private Rigidbody2D rigidbody;
	private Coroutine currentDive;
	private Animator seedAnimator;

	private float reboundingHorizontalSpeedupRate;
	private float m_horizontalSpeed;
	private float reboundTopAltitude;
	private float reboundDuration;

	private RotationController rotationController;

	public float horizontalSpeed
	{
		get => m_horizontalSpeed;
		set
		{
			m_horizontalSpeed = Mathf.Clamp(value, 0, maxBosstedHorizontalSpeed);
		}
	}

	private void Awake()
	{
		seedAnimator = GetComponent<Animator>();
		player = ReInput.players.GetPlayer(0);
		rigidbody = GetComponent<Rigidbody2D>();
		rotationController = GetComponent<RotationController>();
		// ay = g
		// vy = -gt + v0
		// y = -1/2 gt^2 + v0 t + y0
		// 0 = -1/2 gt^2 + equilibriumAltitude
		// g * fallDuration^2 = 2 * equilibriumAltitude
		// g = 2 * equilibriumAltitude / fallDuration^2
		fullReboundDistance = equilibriumAltitude - groundAltitude;
		float fallGravity = 2 * fullReboundDistance / (fallDuration * fallDuration);
		fallGravityScale = fallGravity / Physics2D.gravity.magnitude;

		reboundingHorizontalSpeedupRate = fullReboundSpeedBoost / fullReboundDuration;

		// T = 2*pi*sqrt(m/k)
		// T / (2pi) = sqrt(m/k)
		// T^2 / (2pi)^2 = m/k
		// k = 4m*pi^2 / T^2
		equilibriumSpringCoefficient = 4f * rigidbody.mass * Mathf.PI * Mathf.PI /
			(equilibriumOscillationPeriod * equilibriumOscillationPeriod);

		state = SeedMovementState.Equilibrium;
		horizontalSpeed = baseHorizontalSpeed;
	}

	private void OnEnable()
	{
		currentDive = StartCoroutine(DiveRoutine());
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(new Vector3(transform.position.x - 5, groundAltitude),
			new Vector3(transform.position.x + 5f, groundAltitude));
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(transform.position.x - 5, topAltitude),
			new Vector3(transform.position.x + 5f, topAltitude));
		Gizmos.color = Color.white;
		Gizmos.DrawLine(new Vector3(transform.position.x - 5, equilibriumAltitude),
			new Vector3(transform.position.x + 5, equilibriumAltitude));
		Gizmos.DrawWireCube(new Vector3(transform.position.x, equilibriumAltitude),
			new Vector3(5, equilibriumOscillationAmplitude * 2));
	}

	private void Update()
	{
        if (rigidbody.velocity.x > normalVelocity)
        {
			speedBoost.SetActive(true);
        } else { speedBoost.SetActive(false); }
        if (state != SeedMovementState.Diving
			&& player.GetAnyButtonDown())
		{
			if (currentDive != null)
			{
				StopCoroutine(currentDive);
				currentDive = null;
			}
			currentDive = StartCoroutine(DiveRoutine());
		}
	}

	private void FixedUpdate()
	{
		if (state != SeedMovementState.Equilibrium)
		{
			return;
		}

		horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, baseHorizontalSpeed, horizontalSpeedDecayRate * Time.fixedDeltaTime);

		Vector2 offsetFromEquilibrium = new Vector3(transform.position.x, equilibriumAltitude) - transform.position;
		Vector2 oscillationSpringForce = equilibriumSpringCoefficient * offsetFromEquilibrium;
		rigidbody.AddForce(oscillationSpringForce);
		rigidbody.velocity = new Vector2(horizontalSpeed, rigidbody.velocity.y);
	}

	public void SlowDown(float slowDownFactor)
	{
		rigidbody.velocity *= slowDownFactor;
		horizontalSpeed *= slowDownFactor;
		
		if (state == SeedMovementState.Rising)
		{
			float remainingRiseDistance = reboundTopAltitude - transform.position.y;
			remainingRiseDistance *= slowDownFactor * slowDownFactor;
			reboundTopAltitude = transform.position.y + remainingRiseDistance;
			reboundDuration *= slowDownFactor;
		}
	}

	public void SpeedUp(float speedUpFactor)
	{
		if (state == SeedMovementState.Diving)
		{
			return;
		}

		horizontalSpeed *= speedUpFactor;

		if (state == SeedMovementState.Rising)
		{
			rigidbody.velocity *= speedUpFactor;

			float remainingRiseDistance = reboundTopAltitude - transform.position.y;
			remainingRiseDistance *= speedUpFactor * speedUpFactor;
			reboundTopAltitude += transform.position.y + remainingRiseDistance;
			reboundTopAltitude = Mathf.Min(topAltitude, reboundTopAltitude);
			reboundDuration *= speedUpFactor;
		}
	}

	public void Drop(float duration)
	{
		if (currentDive != null)
		{
			StopCoroutine(currentDive);
			currentDive = null;
		}
		currentDive = StartCoroutine(DropRoutine(duration));
	}

	private IEnumerator DropRoutine(float duration)
	{
		state = SeedMovementState.Diving;
		seedAnimator.SetBool("Dive", true);
		seedAnimator.SetBool("Rise", false);
		seedAnimator.SetBool("Normal", false);

		rigidbody.gravityScale = fallGravityScale;
		yield return new WaitForSeconds(duration);

		IEnumerator returnToEquilibriumEnumerator = ReturnToEquilibriumRoutine();
		while (returnToEquilibriumEnumerator.MoveNext())
		{
			yield return returnToEquilibriumEnumerator.Current;
		}

		state = SeedMovementState.Equilibrium;
		currentDive = null;
	}

	private IEnumerator DiveRoutine()
	{
		state = SeedMovementState.Diving;

		seedAnimator.SetBool("Dive", true);
		seedAnimator.SetBool("Rise", false);
		seedAnimator.SetBool("Normal", false);

		float fallStartAltitude = transform.position.y;
		rigidbody.gravityScale = fallGravityScale;

		while (player.GetButton("Fall"))
		{
			yield return null;
		}

		yield return new WaitForFixedUpdate();

		float distanceFallen = fallStartAltitude - transform.position.y;
		if (distanceFallen > 0)
		{
			// d / t = df / tf
			// d * tf = t * df
			// t = d * tf / df
			reboundDuration = Mathf.Sqrt(distanceFallen / fullReboundDistance) * fullReboundDuration;
			float reboundDistance = fullReboundDistanceMultiplier * distanceFallen;
			reboundTopAltitude = Mathf.Min(topAltitude, transform.position.y + reboundDistance);
			IEnumerator reboundEnumerator = ReboundRoutine();
			while (reboundEnumerator.MoveNext())
			{
				yield return reboundEnumerator.Current;
			}
		}

		IEnumerator returnToEquilibriumEnumerator = ReturnToEquilibriumRoutine();
		while (returnToEquilibriumEnumerator.MoveNext())
		{
			yield return returnToEquilibriumEnumerator.Current;
		}

		state = SeedMovementState.Equilibrium;
		currentDive = null;
	}

	private IEnumerator ReboundRoutine()
	{
		state = SeedMovementState.Rising;
		seedAnimator.SetBool("Rise", true);
		seedAnimator.SetBool("Normal", false);
		seedAnimator.SetBool("Dive", false);

		rigidbody.gravityScale = 0f;

		for (; reboundDuration > 0; reboundDuration -= Time.fixedDeltaTime)
		{
			float velocity = rigidbody.velocity.y;
			if (velocity < 0)
			{
				float newPosition = Mathf.SmoothDamp(transform.position.y,
					reboundTopAltitude, ref velocity, reboundDuration / 3);
				transform.position = new Vector3(transform.position.x, newPosition);
			}
			else
			{
				float newPosition = Mathf.SmoothDamp(transform.position.y,
					reboundTopAltitude, ref velocity, reboundDuration);
				transform.position = new Vector3(transform.position.x, newPosition);
			}

			horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, maxHorizontalSpeed, reboundingHorizontalSpeedupRate * Time.fixedDeltaTime);

			rigidbody.velocity = new Vector2(horizontalSpeed, velocity);
			Debug.DrawLine(transform.position, new Vector3(transform.position.x, reboundTopAltitude));
			yield return new WaitForFixedUpdate();
		}
	}

	private IEnumerator ReturnToEquilibriumRoutine()
	{
		state = SeedMovementState.FallingToEquilibrium;
		seedAnimator.SetBool("Normal", true);
		seedAnimator.SetBool("Dive", false);
		seedAnimator.SetBool("Rise", false);

		float dampDestination = equilibriumAltitude +
			(transform.position.y > equilibriumAltitude ?
				-equilibriumOscillationAmplitude :
				equilibriumOscillationAmplitude);
		float fallDistance = transform.position.y - dampDestination;
		float maxFallDistance = topAltitude - dampDestination;
		// dy/dt = v_avg
		// v_avg = v_start / 2
		// dy/dt = v_start / 2
		// dt = 2dy / v_start
		float dampTime = Mathf.Sqrt(Mathf.Abs(fallDistance) / maxFallDistance) *
			maxFallToEquilibriumDuration;
		for (float dt = 0f; dt < dampTime; dt += Time.fixedDeltaTime)
		{
			float velocity = rigidbody.velocity.y;
			horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, baseHorizontalSpeed, horizontalSpeedDecayRate * Time.fixedDeltaTime);
			float newPosition = Mathf.SmoothDamp(transform.position.y,
				dampDestination, ref velocity, dampTime - dt);
			transform.position = new Vector3(transform.position.x, newPosition);
			rigidbody.velocity = new Vector2(horizontalSpeed, velocity);
			Debug.DrawLine(transform.position, new Vector3(transform.position.x, dampDestination));
			yield return new WaitForFixedUpdate();
		}
	}
}
