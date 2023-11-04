using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Level Generation/Paint Tiles")]
public class LevelPaintTile : ScriptableObject
{
    public TileBase paint;

    public TileBase wall;
    public TileBase floor;
    public TileBase doors;
}
