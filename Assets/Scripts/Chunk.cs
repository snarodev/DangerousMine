using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Chunk
{
    public Vector2Int position;

    public const int ChunkSize = 16;

    public Tile[] tiles;

    public Chunk(Vector2Int position)
    {
        this.position = position;
        tiles = new Tile[ChunkSize * ChunkSize];
    }

    public Tile GetTile(Vector2Int pos)
    {
        pos -= position;

        return tiles[pos.y * ChunkSize + pos.x];
    }

    public void SetTile(Vector2Int pos, Tile tile)
    {
        pos -= position;

        tiles[pos.y * ChunkSize + pos.x] = tile;
    }

    public static Vector2Int MakeChunkPos(Vector2Int pos)
    {
        return new Vector2Int(Mathf.FloorToInt((float)pos.x / ChunkSize) * ChunkSize,
           Mathf.FloorToInt((float)pos.y / ChunkSize) * ChunkSize);
    }
    public static Vector2Int MakeChunkPos(Vector2 pos)
    {
        return new Vector2Int(Mathf.FloorToInt((float)pos.x / ChunkSize) * ChunkSize,
           Mathf.FloorToInt((float)pos.y / ChunkSize) * ChunkSize);
    }
    public static Vector2Int MakeChunkPos(int x,int y)
    {
        return new Vector2Int(Mathf.FloorToInt((float)x / ChunkSize) * ChunkSize,
           Mathf.FloorToInt((float)y / ChunkSize) * ChunkSize);
    }
}
