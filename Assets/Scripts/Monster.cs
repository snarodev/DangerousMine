using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public GameObject healthSlider;
    public Image healthSliderImage;

    public GameObject deathDestroyEffect;


    int health = 5;
    readonly int startHealth = 5;

    Vector3 healthSliderOffset;

    protected Transform player;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        healthSliderOffset = healthSlider.transform.position - transform.position;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    public virtual void Update()
    {
        if (health == startHealth)
            healthSlider.SetActive(false);
        else
            healthSlider.SetActive(true);


        healthSlider.transform.position = transform.position + healthSliderOffset;
        healthSlider.transform.rotation = Quaternion.identity;

        healthSliderImage.fillAmount = (float)health / startHealth;

        if (health <= 0)
        {
            Destroy(gameObject);

            GameObject go = Instantiate(deathDestroyEffect, transform.position, Quaternion.identity);
            Destroy(go, 3);
        }


        if (Vector2.Distance(transform.position, player.position) > 30)
        {
            Destroy(gameObject);
        }
    }
}
