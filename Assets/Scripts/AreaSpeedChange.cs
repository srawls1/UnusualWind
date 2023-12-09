using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpeedChange : MonoBehaviour
{
    private bool ocean = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "Ocean")
        {
            ocean = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
