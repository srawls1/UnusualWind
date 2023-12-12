using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpVolume : MonoBehaviour
{
    [SerializeField, Min(1f)] private float speedUpFactor;
	private AudioSource whooshSfx;

	private void Start()
	{
		whooshSfx = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		whooshSfx.Play();

		SeedInWind seed = collision.GetComponent<SeedInWind>();
		if (seed != null)
		{
			seed.SpeedUp(speedUpFactor);
		}
	}
}
