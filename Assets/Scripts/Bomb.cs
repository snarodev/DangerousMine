using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage;

    public GameObject tileBreakEffect;
    public GameObject explosionPrefab;
    public GameObject droppedGoldPrefab;

    float timer = 2;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
            return;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name == "Tile")
            {
                Vector2Int tilePos = new Vector2Int((int)colliders[i].transform.position.x, (int)colliders[i].transform.position.y);
                Chunk chunk = WorldController.controller.GetOrCreateChunk(tilePos);

                Tile tile = chunk.GetTile(tilePos);
                tile.isAir = true;
                chunk.SetTile(tilePos, tile);

                for (int t = 0; t < tile.goldAmount; t++)
                {
                    GameObject gold = Instantiate(droppedGoldPrefab, new Vector3(tilePos.x + Random.Range(0f, 1f), tilePos.y + Random.Range(0f, 1f), 0), Quaternion.identity);
                    gold.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(1f, 3f)));
                }

                GameObject effect = Instantiate(tileBreakEffect, new Vector3(tilePos.x + Random.Range(0f, 1f), tilePos.y + Random.Range(0f, 1f), 0), Quaternion.identity);
                Destroy(effect, 3);


                WorldController.controller.SetChunk(chunk);

                ChunkRenderer.chunkRenderer.AddDirtyChunk(tilePos);
            }
            else if (colliders[i].tag == "Enemy")
            {
                colliders[i].gameObject.GetComponent<Monster>().TakeDamage(damage);
            }
        }

        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
