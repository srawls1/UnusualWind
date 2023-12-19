using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlowingPetal : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject player;
    private AreaCheck areaCheck;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioSource collectSfx;
    private Animator animator;
    private float collectSpeed = 3f;
    private float moveAwaySpeed = 10f;
    private bool collected = false;
    private bool moveAway = false;
    private float alphaDecrease = .01f;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpriteRenderer glow;
    [SerializeField] private Light2D light2D;
    [SerializeField] private GameObject plantedFlower;
    [SerializeField] private GameObject lilypadPlanted;
    private float plantedFlowerYForest = -4.3f;
    private float plantedFlowerYForest2 = -3.4f;
    private float plantedFlowerYWheat = -5f;
    private float plantedFlowerYOcean = -8f;
    private bool fadeOut = false;
    private int petalPoints = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        areaCheck = player.GetComponent<AreaCheck>();
    }

    private void Update()
    {
        if (collected)
        {
            //Move upwards
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y - 20f), collectSpeed * Time.deltaTime);
        }

        if (moveAway)
        {
            //Move downwards
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + .5f), moveAwaySpeed * Time.deltaTime);
        }

        if (fadeOut)
        {
            //decrease alpha value of sprite
            sprite.color = new Color(1f, 1f, 1f, sprite.color.a - alphaDecrease);
            glow.color = new Color(1f, 1f, 1f, glow.color.a - alphaDecrease);
            //decrease intensity of light
            light2D.intensity -= alphaDecrease;

            if (sprite.color.a <= 0)
            {                
                gameManager.GetComponent<GameManager>().petalCount += petalPoints;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Trigger the animation
            animator.SetTrigger("Collect");

            audioManager.PlayCollectSfx();

            //turn off the collider
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void MoveTowardsPlayer()
    {
        moveAway = false;
        collected = true;
    }

    private void MoveAway()
    {
        moveAway = true;
    }

    private void FadeOut()
    {
        fadeOut = true;
    }

    private void InstantiateFlower()
    {
        if (areaCheck.ocean)
        {
            Instantiate(lilypadPlanted, new Vector2(transform.position.x, plantedFlowerYOcean), Quaternion.identity);
        } else if (areaCheck.wheat)
        {
            Instantiate(plantedFlower, new Vector2(transform.position.x, plantedFlowerYWheat), Quaternion.identity);
        } else if (areaCheck.forest2)
        {
            Instantiate(plantedFlower, new Vector2(transform.position.x, plantedFlowerYForest2), Quaternion.identity);
        } else if (areaCheck.forest1)
        {
            Instantiate(plantedFlower, new Vector2(transform.position.x, plantedFlowerYForest), Quaternion.identity);
        }
    }
}
