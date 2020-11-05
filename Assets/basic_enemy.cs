using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StartDirection
{
    LEFT,
    RIGHT,
}

public class basic_enemy : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2d;
    private SpriteRenderer sprite;
    [SerializeField]
    private StartDirection startDirection = StartDirection.LEFT;
    [SerializeField]
    private int maxDist = 2;
    [SerializeField]
    private int moveSpeed = 1;
    private float startLocation;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        startLocation = transform.position.x;
        if (startDirection == StartDirection.LEFT)
        {
            rb2d.velocity = new Vector2(-moveSpeed, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(moveSpeed, 0);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (transform.position.x < startLocation - maxDist)
        {
            rb2d.velocity = new Vector2(-rb2d.velocity.x, 0);
        }
        else if (transform.position.x > startLocation + maxDist)
        {
            rb2d.velocity = new Vector2(-rb2d.velocity.x, 0);
        }
        else
        {
            rb2d.velocity = rb2d.velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
