using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpSpeed;
    [SerializeField] private LayerMask ground;

    private PlayerActionControls playerActionControls;

    private Rigidbody2D rb;

    private Collider2D col;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerActionControls = new PlayerActionControls();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {

        playerActionControls.Enable();
    }

    private void OnDisable()
    {
        playerActionControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerActionControls.Land.Jump.performed += _ => Jump();
    }
    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        
        }
    }

    private bool IsGrounded()
    {
        Vector2 topLeftPoint = transform.position;
        topLeftPoint.x -= col.bounds.extents.x;
        topLeftPoint.y += col.bounds.extents.y;

        Vector2 bottomRightPoint = transform.position;
        bottomRightPoint.x += col.bounds.extents.x;
        bottomRightPoint.x -= col.bounds.extents.y;


        return Physics2D.OverlapArea(topLeftPoint, bottomRightPoint, ground);
        
    }
    // Update is called once per frame
    void Update()
    {
        Move();


    }

    private void Move()
    {
        //Read the movement value
        float movementInput = playerActionControls.Land.Move.ReadValue<float>();

        //Move the player
        //This is the only format that works. You have to 
        //obtain the current position, assign it to a Vector
        //change the Vector then reassign it to transform.position

        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;

        //Animation
        if (movementInput != 0)
        {
            animator.SetBool("Run", true);
        }
        else animator.SetBool("Run", false);

        //Sprite Flip
        if(movementInput == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput == 1)
        {
            spriteRenderer.flipX = false;
        }
    }
}
