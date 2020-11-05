using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpState
{
    FALLING,
    JUMPING,
    GROUNDED
}

public class movement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2d;
    private SpriteRenderer sprite;
    private float hInput = 0;
    private float maxAirSpeed = 2f;
    private float maxFallSpeed = -10f;
    private JumpState jumpState = JumpState.GROUNDED;
    private RaycastHit2D groundcastHit;
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private int movesspeed = 1;
    [SerializeField]
    private int jumpForce = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(jumpState);
        hInput = Input.GetAxis("Horizontal");

        groundcastHit = DoBoxCast(0.1f);
        if (rb2d.velocity.y < 0.01 && rb2d.velocity.y > -0.01)
        {
            jumpState = JumpState.GROUNDED;
        }

        if (Input.GetKeyDown("space") && jumpState == JumpState.GROUNDED)
        {
            Debug.Log("JUMPING");
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);                  //zero out y velocity so new force is fresh / consistent
            rb2d.AddForce(new Vector2(0, 150*jumpForce));
            jumpState = JumpState.JUMPING;
        }
        if (rb2d.velocity.y < 0)
        {
            jumpState = JumpState.FALLING;
        }
    }

    void FixedUpdate()
    {
        if (hInput != 0)
        {
            HandleHorizontalMovement();
        }
        //Limit fall speed if necesary
        rb2d.velocity = (new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, maxFallSpeed, 50)));
    }

    private void HandleHorizontalMovement()
    {
        rb2d.velocity = new Vector2(hInput*movesspeed, rb2d.velocity.y);
    }

    private RaycastHit2D DoBoxCast(float distanceBelowPlayer)
    {
        RaycastHit2D result;
        //Boxcast slightly below the player, looking for objects on the groundLayerMask
        result = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, distanceBelowPlayer, groundLayerMask);

        //Set debug ray colors based on grounded or not
        Color rayColor;
        if (groundcastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        //Draw rays to visualize the box cast
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + distanceBelowPlayer), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + distanceBelowPlayer), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + distanceBelowPlayer), Vector2.right * (boxCollider2d.bounds.extents.x * 2), rayColor);

        //Return whether or not the boxcast hit ground
        return result;
    }
}
