using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private float jumpMagnitude;

    new private Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        JumpCheck();
    }

    private void JumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 direction = new Vector2(0f, 1f).normalized;
            Vector2 force = direction * jumpMagnitude;
            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
