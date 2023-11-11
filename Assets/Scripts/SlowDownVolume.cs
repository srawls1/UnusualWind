using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownVolume : MonoBehaviour
{
	[SerializeField, Range(0f, 1f)] private float slowDownFactor;

	private void OnTriggerExit2D(Collider2D collision)
	{
		SeedInWind seed = collision.GetComponent<SeedInWind>();
        if (seed != null)
        {
			seed.SlowDown(slowDownFactor);
		}
	}
}
