using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfAfterDuration : MonoBehaviour
{
	[SerializeField] private float duration;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(duration);
		Destroy(gameObject);
	}
}
