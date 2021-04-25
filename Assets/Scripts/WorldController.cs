using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : Controller
{
    public GameObject shopPrefab;
    public GameObject spiderPrefab;

    [HideInInspector]
    public List<Chunk> chunks;

    public static WorldController controller;


    public override void Init()
    {
        chunks = new List<Chunk>();

        controller = this;

        
    }

    public override void Tick()
    {

    }

    public Chunk GetOrCreateChunk(Vector2Int pos)
    {
        Vector2Int chunkPos = Chunk.MakeChunkPos(pos);

        for (int i = 0; i < chunks.Count; i++)
        {
            if (chunks[i].position == chunkPos)
            {
                return chunks[i];
            }
        }

        Chunk chunk = GenerateChunk(pos);

        chunks.Add(chunk);
        return chunk;        
    }

    public void SetChunk(Chunk chunk)
    {
        Vector2Int chunkPos = Chunk.MakeChunkPos(chunk.position);

        for (int i = 0; i < chunks.Count; i++)
        {
            if (chunks[i].position == chunkPos)
            {
                chunks[i] = chunk;
                return;
            }
        }

        chunks.Add(chunk);
    }

    Chunk GenerateChunk(Vector2Int pos)
    {
        Chunk chunk = new Chunk(pos);

        for (int i = 0; i < chunk.tiles.Length; i++)
        {
            Vector2Int tilePos = new Vector2Int(i % Chunk.ChunkSize, (int)(i / Chunk.ChunkSize)) + pos;


            if (tilePos.y > 0)
                chunk.tiles[i].isAir = true;
            else
            {
                chunk.tiles[i].isAir = false;


                



                float caveNoise = Mathf.PerlinNoise(tilePos.x * 0.1f + 2000, tilePos.y * 0.1f);

                if (caveNoise > Mathf.Lerp(1, 0.4f, Mathf.Abs(tilePos.y) / 400f))
                    chunk.tiles[i].isAir = true;

                if (Mathf.Abs(tilePos.x) % 100 == 0 && Mathf.Abs(tilePos.y) % 100 == 0)
                {
                    GameObject go = Instantiate(shopPrefab, new Vector3(tilePos.x, tilePos.y - 5), Quaternion.identity);

                    chunk.tiles[i].isAir = true;
                }
                else if ((Mathf.Abs(tilePos.x + 0) % 100 == 0 && Mathf.Abs(tilePos.y + 6) % 100 == 0) ||
                    (Mathf.Abs(tilePos.x - 1) % 100 == 0 && Mathf.Abs(tilePos.y + 6) % 100 == 0) ||
                    (Mathf.Abs(tilePos.x - 2) % 100 == 0 && Mathf.Abs(tilePos.y + 6) % 100 == 0))
                {
                    chunk.tiles[i].stone = true;
                }
                else if (Mathf.Abs(tilePos.x) % 100 > 5 || Mathf.Abs(tilePos.y) % 100 > 5)
                {
                    float oreNoise = Mathf.PerlinNoise(tilePos.x * 0.1f + 1000, tilePos.y * 0.1f);

                    if (oreNoise > Mathf.Lerp(1, 0.8f, Mathf.Abs(tilePos.y) / 1000f))
                        chunk.tiles[i].goldAmount = 4;
                    else if (oreNoise > Mathf.Lerp(1, 0.6f, Mathf.Abs(tilePos.y) / 800f))
                        chunk.tiles[i].goldAmount = 3;
                    else if (oreNoise > Mathf.Lerp(1, 0.5f, Mathf.Abs(tilePos.y) / 600f))
                        chunk.tiles[i].goldAmount = 2;
                    else if (oreNoise > Mathf.Lerp(1, 0.4f, Mathf.Abs(tilePos.y) / 200f))
                        chunk.tiles[i].goldAmount = 1;
                    else
                        chunk.tiles[i].goldAmount = 0;


                    if (caveNoise > Mathf.Lerp(1, 0.7f, Mathf.Abs(tilePos.y) / 500f))
                    {
                        GameObject go = Instantiate(spiderPrefab, new Vector3(tilePos.x, tilePos.y) + new Vector3(0.5f, 0.5f), Quaternion.identity);
                    }
                }
                else
                {
                    chunk.tiles[i].isAir = true;
                }

                



            }
        }

        return chunk;
    }
}
