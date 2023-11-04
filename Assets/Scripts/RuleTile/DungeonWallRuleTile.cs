using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class DungeonWallRuleTile : RuleTile<DungeonWallRuleTile.Neighbor> {
    public TileBase[] floorTile;
    public TileBase[] wallTiles;
    public TileBase[] doorTiles;
    public TileBase voidTile;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Floor = 3;
        public const int Wall = 4;
        public const int Door = 5;
        public const int Null = 6;
        //public const int Others = 7;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Floor: return floorTile.Contains(tile);
            case Neighbor.Wall: return tile == wallTiles.Contains(tile);
            case Neighbor.Door: return tile == doorTiles.Contains(tile);
            case Neighbor.Null: return tile == null || tile == voidTile;
        }

        // Default rule tiles
        return base.RuleMatch(neighbor, tile);
    }
}