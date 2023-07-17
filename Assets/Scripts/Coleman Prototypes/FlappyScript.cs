using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyScript : MonoBehaviour
{
    public float velocity = 1;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Jump
            rb.velocity = Vector2.up * velocity;
        }
    }
}
