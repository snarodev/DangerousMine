using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Controller
{
    public float speed = 10;
    public float jumpPower = 10;
    public float airDrag = 10;
    public float groundDrag;
    public float airControl = 0.2f;
    public Vector2 groundCheckOffset = new Vector2(0, -0.5f);


    bool onGround = false;
    

    Rigidbody2D rb;
    Animator anim;
    

    bool jump;
    float horizontal = 0;

    public override void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (!jump)
            jump = Input.GetButtonDown("Jump");

        
    }

    public override void Tick()
    {
        Movement();
       
    }

    void Movement()
    {
        Vector2 force = new Vector2(horizontal * speed, 0);

        Collider2D hit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + groundCheckOffset, new Vector2(0.3f, 0.3f), 0);

        if (hit != null)
            onGround = true;
        else
            onGround = false;


        if (jump)
        {
            jump = false;

            if (onGround)
                force.y = jumpPower;
        }

        if (onGround)
            rb.drag = groundDrag;
        else
        {
            rb.drag = airDrag;
            force.x *= airControl;
        }


        rb.AddForce(force, ForceMode2D.Force);

        if (onGround)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    
    private void OnDrawGizmos()
    {
        if (onGround)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y) + groundCheckOffset, new Vector2(0.3f, 0.3f));
    }
}
