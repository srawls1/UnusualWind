using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
	[SerializeField] private float windSpeed;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SeedInWind seed = collision.GetComponent<SeedInWind>();
		if (seed)
		{
			seed.RegisterWind(this);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		SeedInWind seed = collision.GetComponent<SeedInWind>();
		if (seed)
		{
			seed.DeregisterWind(this);
		}
	}

	public Vector2 GetWindVelocityAtPoint(Vector2 point)
	{
		return windSpeed * Vector2.right;
	}
}
