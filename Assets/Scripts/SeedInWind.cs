using System.Collections;
using UnityEngine.Serialization;
using UnityEngine;
using Rewired;

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

	public float horizontalSpeed
	{
		get => m_horizontalSpeed;
		set
		{
			m_horizontalSpeed = Mathf.Clamp(value, baseHorizontalSpeed, maxHorizontalSpeed);
		}
	}

	private void Awake()
	{
		seedAnimator = GetComponent<Animator>();
		player = ReInput.players.GetPlayer(0);
		rigidbody = GetComponent<Rigidbody2D>();
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

		horizontalSpeed -= horizontalSpeedDecayRate * Time.fixedDeltaTime;

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
			reboundTopAltitude += remainingRiseDistance;
		}
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

		state = SeedMovementState.Rising;
		seedAnimator.SetBool("Rise", true);
		seedAnimator.SetBool("Normal", false);
		seedAnimator.SetBool("Dive", false);

		rigidbody.gravityScale = 0f;

		yield return new WaitForFixedUpdate();

		float distanceFallen = fallStartAltitude - transform.position.y;
		if (distanceFallen > 0)
		{
			// d / t = df / tf
			// d * tf = t * df
			// t = d * tf / df
			float reboundDuration = Mathf.Sqrt(distanceFallen / fullReboundDistance) * fullReboundDuration;
			float reboundDistance = fullReboundDistanceMultiplier * distanceFallen;
			reboundTopAltitude = Mathf.Min(topAltitude, transform.position.y + reboundDistance);

			for (float dt = 0f; dt < reboundDuration; dt += Time.fixedDeltaTime)
			{
				float velocity = rigidbody.velocity.y;
				if (velocity < 0) { float newPosition = Mathf.SmoothDamp(transform.position.y,
						reboundTopAltitude, ref velocity, (reboundDuration - dt) / 3);
					transform.position = new Vector3(transform.position.x, newPosition); }
				else { float newPosition = Mathf.SmoothDamp(transform.position.y,
						reboundTopAltitude, ref velocity, reboundDuration - dt);
					transform.position = new Vector3(transform.position.x, newPosition); }

				horizontalSpeed += reboundingHorizontalSpeedupRate * Time.fixedDeltaTime;

				rigidbody.velocity = new Vector2(horizontalSpeed, velocity);
				Debug.DrawLine(transform.position, new Vector3(transform.position.x, reboundTopAltitude));
				yield return new WaitForFixedUpdate();
			}
		}

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
			horizontalSpeed -= horizontalSpeedDecayRate * Time.fixedDeltaTime;
			float newPosition = Mathf.SmoothDamp(transform.position.y,
				dampDestination, ref velocity, dampTime - dt);
			transform.position = new Vector3(transform.position.x, newPosition);
			rigidbody.velocity = new Vector2(horizontalSpeed, velocity);
			Debug.DrawLine(transform.position, new Vector3(transform.position.x, dampDestination));
			yield return new WaitForFixedUpdate();
		}

		state = SeedMovementState.Equilibrium;
		currentDive = null;
	}
}
