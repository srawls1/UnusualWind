using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleStartScript : MonoBehaviour
{
    private float alphaDecrease = 0.01f;
    // Update is called once per frame
    void Update()
    {
        //If any button is pressed, fade object spite out by getting sprite renderer and changing alpha
        if (Input.anyKey)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(1f, 1f, 1f, sprite.color.a - alphaDecrease);

            //If alpha is 0, destroy object
            if (sprite.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
