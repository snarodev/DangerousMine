using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedGold : MonoBehaviour
{
    Transform player;

    bool lockOnPlayer;

    public GameObject[] collectEffect;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (lockOnPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 6 * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) < 0.5f)
            {
                PlayerInventory.playerInventory.GoldCollected(1);
                Destroy(gameObject);

                GameObject go = Instantiate(collectEffect[Random.Range (0,collectEffect.Length)], transform.position, Quaternion.identity);
                Destroy(go, 2);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < 2f)
            {
                Destroy(GetComponent<BoxCollider2D>());
                Destroy(GetComponent<Rigidbody2D>());

                lockOnPlayer = true;
            }
        }
    }
}
