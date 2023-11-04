using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Level Generation/Settings")]
public class LevelGenerationSettings : ScriptableObject
{
    [Serializable]
    public class TileData
    {
        public TileBase tile;
    }

    public TileData[] tiles;
    public float perlinNoiseScale;
    public Vector2Int size;
}
