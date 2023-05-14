using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedInWind : MonoBehaviour
{
	//private float 

	private List<WindZone> windZones;

	private void Awake()
	{
		windZones = new List<WindZone>();
	}

	private void Update()
	{
		
	}

	public void RegisterWind(WindZone zone)
	{
		windZones.Add(zone);
	}

	public void DeregisterWind(WindZone zone)
	{
		windZones.Remove(zone);
	}
}
