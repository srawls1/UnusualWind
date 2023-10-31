using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public GameObject particleEffectPrefab; // Assign your particle effect prefab here

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the collision point
        Vector3 collisionPoint = collision.GetContact(0).point;

        // Instantiate the particle effect at the collision point
        Instantiate(particleEffectPrefab, collisionPoint, Quaternion.identity);
    }
}
