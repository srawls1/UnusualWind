using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleOnTriggerLeave : MonoBehaviour
{
    [SerializeField] private GameObject particle;

	private void OnTriggerExit2D(Collider2D collision)
	{
		//if (collision.CompareTag("Player"))
		{
			Vector2 velocity = collision.attachedRigidbody.velocity;
			Instantiate(particle, collision.attachedRigidbody.position,
				Quaternion.FromToRotation(Vector2.up, velocity));
		}
	}
}
