using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class SeedInWind : MonoBehaviour
{
	[SerializeField] private float facingBlendSpeed;
	[SerializeField] private float dragCoefficientBlendSpeed;
	[SerializeField] private float openDragCoefficient;
	[SerializeField] private float closedDragCoefficient;
	[SerializeField] private float openGravityScale;
	[SerializeField] private float closedGravityScale;

	private float dragCoefficient;
	private float gravityScale;
	private Vector2 facingVector;
	private Player player;
	new private Rigidbody2D rigidbody;
	private List<WindZone> windZones;

	private void Awake()
	{
		dragCoefficient = closedDragCoefficient;
		facingVector = Vector2.up;
		player = ReInput.players.GetPlayer(0);
		rigidbody = GetComponent<Rigidbody2D>();
		windZones = new List<WindZone>();
	}

	private void Update()
	{
		UpdateDragCoefficient();
		UpdateDirection();
	}

	private void FixedUpdate()
	{
		Vector2 windSpeed = GetAverageWindSpeed();
		Vector2 diffFromWindSpeed = windSpeed - rigidbody.velocity;
		float windCaught = Vector2.Dot(facingVector, diffFromWindSpeed) * dragCoefficient;
		windCaught = Mathf.Max(0f, windCaught);
		Debug.Log($"windSpeed={windSpeed}, diffFromWindSpeed={diffFromWindSpeed}, facing={facingVector}, windCaught={windCaught}");
		rigidbody.AddForce(windCaught * facingVector);
	}

	private void UpdateDirection()
	{
		Vector2 directionInput = player.GetAxis2D("Horizontal", "Vertical");
		if (directionInput.sqrMagnitude > 0.2f)
		{
			facingVector = directionInput.normalized;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation,
			Quaternion.Euler(0f, 0f, Mathf.Atan2(-facingVector.x, facingVector.y) * Mathf.Rad2Deg),
			Time.deltaTime * facingBlendSpeed);
	}

	private void UpdateDragCoefficient()
	{
		bool open = player.GetButton("Open");
		float targetDrag = open ? openDragCoefficient : closedDragCoefficient;
		dragCoefficient = Mathf.Lerp(dragCoefficient, targetDrag, Time.deltaTime * dragCoefficientBlendSpeed);
		float targetGravity = open ? openGravityScale : closedGravityScale;
		gravityScale = Mathf.Lerp(gravityScale, targetGravity, Time.deltaTime * dragCoefficient);
		rigidbody.gravityScale = gravityScale;
	}

	private Vector2 GetAverageWindSpeed()
	{
		Vector2 totalWind = Vector2.zero;
		for (int i = 0; i < windZones.Count; ++i)
		{
			totalWind += windZones[i].GetWindVelocityAtPoint(transform.position);
		}

		return totalWind / Mathf.Max(1, windZones.Count);
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
