using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedUp : MonoBehaviour
{
	[SerializeField, Range(1, 10)] private float timeScale;

	private void OnEnable()
	{
		Time.timeScale = timeScale;
	}

	private void OnDisable()
	{
		Time.timeScale = 1f;
	}
}
