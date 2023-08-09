using System.Collections;
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

	[SerializeField] private float horizontalSpeed = 4f;
	[SerializeField] private float groundAltitude;
	[SerializeField] private float equilibriumAltitude;
	[SerializeField] private float topAltitude;
	[SerializeField] private float equilibriumOscillationPeriod = 3f;
	[SerializeField] private float equilibriumOscillationAmplitude = 1f;
	[SerializeField] private float fallDuration;
	[SerializeField] private float fullReboundDuration;
	[SerializeField] private float fullReboundDistanceMultiplier = 1.2f;
	[SerializeField] private float maxFallToEquilibriumDuration;

	private float fallGravityScale;
	private float fullReboundDistance;
	private float equilibriumSpringCoefficient;
	private SeedMovementState state;

	private Player player;
	new private Rigidbody2D rigidbody;
	private Coroutine currentDive;

	private void Awake()
	{
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

		// T = 2*pi*sqrt(m/k)
		// T / (2pi) = sqrt(m/k)
		// T^2 / (2pi)^2 = m/k
		// k = 4m*pi^2 / T^2
		equilibriumSpringCoefficient = 4f * rigidbody.mass * Mathf.PI * Mathf.PI /
			(equilibriumOscillationPeriod * equilibriumOscillationPeriod);

		state = SeedMovementState.Equilibrium;
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
			&& player.GetButtonDown("Fall"))
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

		Vector2 offsetFromEquilibrium = new Vector3(transform.position.x, equilibriumAltitude) - transform.position;
		Vector2 oscillationSpringForce = equilibriumSpringCoefficient * offsetFromEquilibrium;
		rigidbody.AddForce(oscillationSpringForce);
		rigidbody.velocity = new Vector2(horizontalSpeed, rigidbody.velocity.y);
	}

	private IEnumerator DiveRoutine()
	{
		state = SeedMovementState.Diving;

		float fallStartAltitude = transform.position.y;
		rigidbody.gravityScale = fallGravityScale;

		while (player.GetButton("Fall"))
		{
			yield return null;
		}

		state = SeedMovementState.Rising;
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
			float reboundTopAltitude = Mathf.Min(topAltitude, transform.position.y + reboundDistance);

			for (float dt = 0f; dt < reboundDuration; dt += Time.fixedDeltaTime)
			{
				float velocity = rigidbody.velocity.y;
				float newPosition = Mathf.SmoothDamp(transform.position.y,
					reboundTopAltitude, ref velocity, reboundDuration - dt);
				transform.position = new Vector3(transform.position.x, newPosition);
				rigidbody.velocity = new Vector2(horizontalSpeed, velocity);
				Debug.DrawLine(transform.position, new Vector3(transform.position.x, reboundTopAltitude));
				yield return new WaitForFixedUpdate();
			}
		}

		state = SeedMovementState.FallingToEquilibrium;

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

	public void RegisterWind(WindZone zone)
	{
	}

	public void DeregisterWind(WindZone zone)
	{
	}
}
