using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tile
{
    public bool isAir;

    public byte goldAmount;

    public bool stone;

    public static Vector2Int MakeTilePos(Vector2 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x),
           Mathf.FloorToInt(pos.y));
    }
}
