using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Monster
{
    public float jumpInterval = 3;
    public float jumpPower = 4;

    public float outOfControlTime = 4;

    float lastJump;

    Rigidbody2D rb;

    bool outOfControl = false;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }



    public override void Update()
    {
        base.Update();

        if (outOfControl)
        {
            if (lastJump + outOfControlTime < Time.time)
            {
                lastJump = Time.time;

                outOfControl = false;

                rb.freezeRotation = true;

                transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            if (lastJump + jumpInterval < Time.time)
            {
                lastJump = Time.time;


                if (player.position.x - transform.position.x > 0)
                    rb.AddForce(new Vector2(jumpPower, jumpPower), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(-jumpPower, jumpPower), ForceMode2D.Impulse);
            }
        }

        if (rb.velocity.x > 0.2f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity.x < -0.2f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {
            rb.freezeRotation = false;

            outOfControl = true;

            lastJump = Time.time;


            PlayerHealth.playerHealth.TakeDamage(10);
        }
    }
}
