using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;

    public GameObject playerDeathEffect;

    public GameObject[] playerHurtEffects;

    public static PlayerHealth playerHealth;

    int health = 100;

    bool dead = false;

    void Start()
    {
        playerHealth = this;
    }

    void Update()
    {
        if (dead)
            return;

        healthBar.fillAmount = health / 100f;

        if (health <= 0)
        {
            UIController.ui.OpenPanel<GameoverPanel>();

            dead = true;

            GetComponent<SpriteRenderer>().enabled = false;

            GameObject go = Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
            Destroy(go, 3);
        }
    }

    public void TakeDamage(int amount)
    {
        if (dead)
            return;

        health -= amount;

        GameObject go = Instantiate(playerHurtEffects[Random.Range(0, playerHurtEffects.Length)], transform.position, Quaternion.identity);
        Destroy(go, 2);
    }

    public void Revive()
    {
        dead = false;
        health = 100;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
