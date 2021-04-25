using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;

    bool hasHit = false;

    float startSleep = 0.2f;

    public int damage = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 60);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (startSleep > 0)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Monster>().TakeDamage(damage);
            transform.SetParent(collision.transform);
            
        }
        rb.velocity = Vector2.zero;
        hasHit = true;

        GetComponent<Rigidbody2D>().isKinematic = true;
        Destroy(this);

        Destroy(gameObject, 10);
    }

    private void Update()
    {
        startSleep -= Time.deltaTime;


        if (!hasHit)
        {

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
        }
    }


}
