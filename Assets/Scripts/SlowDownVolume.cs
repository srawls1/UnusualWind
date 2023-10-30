using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownVolume : MonoBehaviour
{
	[SerializeField] private float slowDownFactor;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SeedInWind seed = collision.GetComponent<SeedInWind>();
        if (seed != null)
        {
			seed.SlowDown(slowDownFactor);
		}
	}
}
