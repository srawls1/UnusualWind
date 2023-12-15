using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownVolume : MonoBehaviour
{
	[SerializeField, Range(0f, 1f)] private float slowDownFactor;
	[SerializeField] private float destroyWaitTime = .5f;
	public AudioSource[] sfx;
    public bool constantSlow;

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
		if (enabled || constantSlow)
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
		if (sfx == null)
		{
			return;
        }

        //if (sfx[0].isPlaying), return (don't play sfx)
        for (int i = 0; i < sfx.Length - 1; i++)
        {
            if (sfx[i].isPlaying)
            {
                return;
            }
        }
        
        //random range between length of sfx array
        int randomSfx = Random.Range(0, sfx.Length - 1);
		sfx[randomSfx].Play();
    }

	private IEnumerator destroyVolume(float destroyWaitTime)
	{
        yield return new WaitForSeconds(destroyWaitTime);
		enabled = false;
    }
}
