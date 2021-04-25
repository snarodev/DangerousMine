using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer : Controller
{
    Camera cam;

    List<DisplayChunk> displayChunks = new List<DisplayChunk>();

    Sprite[] goldDirt;

    Sprite stone;
    

    List<Vector2Int> dirtyChunks = new List<Vector2Int>();

    public static ChunkRenderer chunkRenderer;

    public override void Init()
    {
        chunkRenderer = this;
        cam = Camera.main;
        goldDirt = new Sprite[5];
        goldDirt[0] = Resources.Load<Sprite>("Dirt");
        goldDirt[1] = Resources.Load<Sprite>("Dirt_Gold1");
        goldDirt[2] = Resources.Load<Sprite>("Dirt_Gold2");
        goldDirt[3] = Resources.Load<Sprite>("Dirt_Gold3");
        goldDirt[4] = Resources.Load<Sprite>("Dirt_Gold4");

        stone = Resources.Load<Sprite>("Stone");
    }

    bool lastFrame = false;

    public override void Tick()
    {
        Vector3 bottomLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));


        lastFrame = !lastFrame;
        for (int x = (int)bottomLeftCorner.x - 1; x < topRightCorner.x + Chunk.ChunkSize; x += Chunk.ChunkSize)
        {
            for (int y = (int)bottomLeftCorner.y - 1; y < topRightCorner.y + Chunk.ChunkSize; y += Chunk.ChunkSize)
            {
                Vector2Int chunkPos = Chunk.MakeChunkPos(x, y);

                DisplayChunk displayChunk = GetDisplayChunk(chunkPos);

                if (displayChunk == null)
                {
                    Chunk chunk = WorldController.controller.GetOrCreateChunk(chunkPos);

                    displayChunk = GenerateDisplayChunk(chunk);
                }
                displayChunk.lastFrame = lastFrame;
            }
        }

        for (int i = 0; i < displayChunks.Count; i++)
        {
            if (displayChunks[i].lastFrame != lastFrame)
            {
                Vector2Int chunkPos = Chunk.MakeChunkPos((int)displayChunks[i].transform.position.x, (int)displayChunks[i].transform.position.y);
                Chunk chunk = WorldController.controller.GetOrCreateChunk(chunkPos);
                DestroyDisplayChunk(chunk);
            }
        }


        for (int i = 0; i < dirtyChunks.Count; i++)
        {
            DisplayChunk displayChunk = GetDisplayChunk(dirtyChunks[i]);

            if (displayChunk != null)
            {
                Chunk chunk = WorldController.controller.GetOrCreateChunk(dirtyChunks[i]);

                UpdateDisplayChunk(chunk);
            }
        }
        dirtyChunks.Clear();
    }

    DisplayChunk GetDisplayChunk(Vector2Int pos)
    {
        for (int i = 0; i < displayChunks.Count; i++)
        {
            if (displayChunks[i].transform.position.x == pos.x &&
                displayChunks[i].transform.position.y == pos.y)
                return displayChunks[i];
        }

        return null;
    }

    DisplayChunk GenerateDisplayChunk(Chunk chunk)
    {
        GameObject displayChunkParent = new GameObject("DisplayChunk",typeof (DisplayChunk));
        displayChunkParent.transform.position = new Vector3(chunk.position.x, chunk.position.y, 0);

        for (int i = 0; i < chunk.tiles.Length; i++)
        {
            GameObject go = new GameObject("Tile",typeof (SpriteRenderer),typeof (BoxCollider2D));

            go.GetComponent<BoxCollider2D>().offset = new Vector2(0.5f, 0.5f);
            go.GetComponent<BoxCollider2D>().size = Vector2.one;

            go.transform.SetParent(displayChunkParent.transform);

            if (chunk.tiles[i].stone)
            {
                go.GetComponent<SpriteRenderer>().sprite = stone;
            }
            else if (!chunk.tiles[i].isAir)
            {
                go.GetComponent<SpriteRenderer>().sprite = goldDirt[chunk.tiles[i].goldAmount];
            }
            else
            {
                go.GetComponent<BoxCollider2D>().enabled = false;
            }


            if (((i % Chunk.ChunkSize) + (int)(i / Chunk.ChunkSize)) % 2 == 0)
                go.GetComponent<SpriteRenderer>().color = new Color (0.7f,0.7f,0.7f);

            go.transform.position = new Vector3(i % Chunk.ChunkSize,(int)(i / Chunk.ChunkSize)) + displayChunkParent.transform.position;
        }

        displayChunks.Add(displayChunkParent.GetComponent<DisplayChunk>());

        return displayChunkParent.GetComponent<DisplayChunk>();
    }

    void UpdateDisplayChunk(Chunk chunk)
    {
        DisplayChunk displayChunkParent = GetDisplayChunk(chunk.position);


        if (displayChunkParent == null)
            return;

        // Destroy all children
        foreach (Transform child in displayChunkParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < chunk.tiles.Length; i++)
        {
            GameObject go = new GameObject("Tile", typeof(SpriteRenderer), typeof(BoxCollider2D));

            go.GetComponent<BoxCollider2D>().offset = new Vector2(0.5f, 0.5f);
            go.GetComponent<BoxCollider2D>().size = Vector2.one;

            go.transform.SetParent(displayChunkParent.transform);

            if (chunk.tiles[i].stone)
            {
                go.GetComponent<SpriteRenderer>().sprite = stone;
            }
            else if (!chunk.tiles[i].isAir)
            {
                go.GetComponent<SpriteRenderer>().sprite = goldDirt[chunk.tiles[i].goldAmount];
            }
            else
            {
                go.GetComponent<BoxCollider2D>().enabled = false;
            }

            if (((i % Chunk.ChunkSize) + (int)(i / Chunk.ChunkSize)) % 2 == 0)
                go.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);

            go.transform.position = new Vector3(i % Chunk.ChunkSize, (int)(i / Chunk.ChunkSize)) + displayChunkParent.transform.position;
        }
    }

    void DestroyDisplayChunk(Chunk chunk)
    {
        DisplayChunk displayChunk = GetDisplayChunk(chunk.position);

        displayChunks.Remove(displayChunk);

        if (displayChunk != null)
        {
            foreach (Transform child in displayChunk.transform)
            {
                Destroy(child.gameObject);
            }

            Destroy(displayChunk.gameObject);
        }
    }

    public void AddDirtyChunk(Vector2Int position)
    {
        if (!dirtyChunks.Contains (Chunk.MakeChunkPos (position)))
            dirtyChunks.Add(Chunk.MakeChunkPos (position));
    }   

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(worldPos, Vector3.one);

            Vector2Int tilePos = Tile.MakeTilePos(new Vector2(worldPos.x, worldPos.y));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(new Vector3(tilePos.x, tilePos.y), Vector3.one);


            Vector2Int chunkPos = Chunk.MakeChunkPos(new Vector2(worldPos.x, worldPos.y));

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3 (chunkPos.x, chunkPos.y), Vector3.one);





            Vector3 bottomLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 topRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

            Gizmos.color = Color.black;
            Gizmos.DrawLine(bottomLeftCorner, topRightCorner);
        }
    }
}
