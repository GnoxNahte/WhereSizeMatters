using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap floorTilemap;

    public LevelGenerationSettings settings;
    [Range(0.001f, 0.5f)]
    public float scale;
    public Vector2Int offset;
    [Range(-1, 1)]
    public float threshold;

    [BurstCompile]
    struct MapGenerationJob : IJobParallelFor
    {
        public Vector2Int size;
        public float noiseScale;
        public Vector2Int offset;
        public float threshold;
        public NativeArray<ushort> mapChunk;

        [BurstCompile]
        public void Execute(int index)
        {
            // Different noise supported by Unity:
            // - snoise
            float value = noise.snoise(new float2(index % size.x + offset.x, index / size.y + offset.y) * noiseScale);
            //Unity.Mathematics.Random rand = new Unity.Mathematics.Random();
            if (value < threshold)
                mapChunk[index] = 0;
            else
                mapChunk[index] = 1;
        }
    }

    void GenerateLevel()
    {
        // Start with a clean level
        ClearMap();

        // Set tiles for floor
    }

    [ContextMenu("Generate Map Job")]
    void GenerateMapJob()
    {
        double startTime = Time.realtimeSinceStartupAsDouble;
        settings.perlinNoiseScale = scale;
        Vector2Int mapSize = settings.size; // To shorten the code
        Vector2Int halfSize = mapSize / 2;
        BoundsInt bounds = new BoundsInt(
            -halfSize.x, -halfSize.y, 0, // Position
            mapSize.x, mapSize.y, 1      // Size
        );

        var array = new NativeArray<ushort>(mapSize.x * mapSize.y, Allocator.TempJob);
        // Init Job
        var job = new MapGenerationJob()
        {
            mapChunk = array,
            size = mapSize,
            noiseScale = scale,
            offset = offset,
            threshold = threshold,
        };
        JobHandle handle = job.Schedule(array.Length, 32);
        handle.Complete();

        var tileArray = new TileBase[mapSize.x * mapSize.y];
        for (int x = 0; x < mapSize.x; ++x)
        {
            for (int y = 0; y < mapSize.y; ++y)
            {
                int index = x * mapSize.x + y;
                tileArray[index] = settings.tiles[array[index]].tile;
            }
        }
        array.Dispose();

        tilemap.SetTilesBlock(bounds, tileArray);
        print("Time taken: " + (Time.realtimeSinceStartupAsDouble - startTime));

    }

    [ContextMenu("Clear Map")]
    void ClearMap()
    {
        tilemap.ClearAllTiles();
    }

    [ContextMenu("Clear Editor Map")]
    void ClearEditorMap()
    {
        tilemap.ClearAllEditorPreviewTiles();
    }

    private void OnValidate()
    {
        ClearMap();
        GenerateMapJob();
    }
}
