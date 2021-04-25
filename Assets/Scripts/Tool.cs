using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/ToolAsset")]
public class Tool : ScriptableObject
{
    public string displayName;
    public int id;
    public int maxDurability;

    public int price;
    public int damage;
    public float rechargeTime;

    public Sprite sprite;
}
