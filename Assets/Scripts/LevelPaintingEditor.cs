using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelPaintingEditor : MonoBehaviour
{
    [SerializeField] Tilemap paintTilemap;
    [SerializeField] Tilemap doorTilemap;
    [SerializeField] Tilemap wallTilemap;
    [SerializeField] Tilemap floorTilemap;

    [SerializeField] LevelPaintTile doorPaint;
    [SerializeField] LevelPaintTile wallPaint;
    [SerializeField] LevelPaintTile floorPaint;

    [ContextMenu("Subscribe to Tile Change")]
    public void SubscribeToTileChange()
    {
        Tilemap.tilemapTileChanged += OnTilemapTileChanged;
    }

    [ContextMenu("Unsubscribe to Tile Change")]
    public void UnsubscribeToTileChange()
    {
        Tilemap.tilemapTileChanged -= OnTilemapTileChanged;
    }

    private void OnTilemapTileChanged(Tilemap tilemap, Tilemap.SyncTile[] tiles)
    {
        if (tilemap != paintTilemap)
            return;

        foreach(Tilemap.SyncTile tile in tiles)
        {
            ReplaceTiles(tile.tile, tile.position);
        }
    }

    [ContextMenu("Replace Paint")]
    public void ReplacePaintTiles()
    {
        double startTime = Time.realtimeSinceStartupAsDouble;

        paintTilemap.CompressBounds();

        BoundsInt bounds = paintTilemap.cellBounds;
        TileBase[] tiles = paintTilemap.GetTilesBlock(bounds);

        for (int i = 0; i < tiles.Length; ++i)
        {
            // TODO: Make it cleaner, faster
            TileBase tile = tiles[i];
            Vector3Int position = new Vector3Int(i % bounds.size.x, i / bounds.size.x) + bounds.position;
            print("Position: " +  position);

            ReplaceTiles(tile, position);
        }

        print("Time taken: " + (Time.realtimeSinceStartupAsDouble - startTime));
    }

    private void ReplaceTiles(TileBase paintTile, Vector3Int position)
    {
        if (paintTile == null)
        {
            return;
        }
        else if (paintTile == wallPaint.paint)
        {
            ReplaceTiles(wallPaint, position);
        }
        else if (paintTile == floorPaint.paint)
        {
            ReplaceTiles(floorPaint, position);
        }
    }

    private void ReplaceTiles(LevelPaintTile tile, Vector3Int position)
    {
        wallTilemap.SetTile(position, tile.wall);
        floorTilemap.SetTile(position, tile.floor);
        doorTilemap.SetTile(position, tile.doors);
    }
}
