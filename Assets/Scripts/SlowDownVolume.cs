using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownVolume : MonoBehaviour
{
	[SerializeField, Range(0f, 1f)] private float slowDownFactor;
	[SerializeField] private float destroyWaitTime = .5f;
	private AudioSource sfx;

	private void Awake()
	{
        sfx = GetComponent<AudioSource>();
    }

	/*private void OnTriggerExit2D(Collider2D collision)
	{
		SeedInWind seed = collision.GetComponent<SeedInWind>();
        if (seed != null)
        {
			seed.SlowDown(slowDownFactor);
		}

		StartCoroutine(destroyVolume(destroyWaitTime));
	}*/

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (enabled)
		{
            SeedInWind seed = collision.GetComponent<SeedInWind>();
            if (seed != null)
            {
                seed.SlowDown(1 - (1 - slowDownFactor) * .027f);
            }

            StartCoroutine(destroyVolume(destroyWaitTime));
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		sfx.Play();
    }

	private IEnumerator destroyVolume(float destroyWaitTime)
	{
        yield return new WaitForSeconds(destroyWaitTime);
		enabled = false;
    }
}
