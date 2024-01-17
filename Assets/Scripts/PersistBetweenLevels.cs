using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistBetweenLevels : MonoBehaviour
{
	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
}
