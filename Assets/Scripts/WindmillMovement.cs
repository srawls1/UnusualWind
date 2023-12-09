using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillMovement : MonoBehaviour
{
    public int movementSpeed; 

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, movementSpeed * Time.deltaTime);
    }
}