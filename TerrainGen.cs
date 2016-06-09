using UnityEngine;
using System.Collections;
using SimplexNoise;


public class TerrainGen
{

    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 4;

    float stoneMountainHeight = 12;
    float stoneMountainFrequency = 0.00008f;
    float stoneMinHeight = -12;

    float dirtBaseHeight = 1;
    float dirtNoise = 0.05f;
    float dirtNoiseHeight = 2;

    float treeFrequency = 0.2f;
    int treeDensity = 3;

    float grassFrequency = 0.8f;
    int grassDensity = 10;

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.chunkSize + 3; x++)
        {
            for (int z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }


    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
            if (y <= stoneHeight)
            {
                SetBlock(x, y, z, new Block(), chunk);
            }
            else if (y <= dirtHeight)
            {
                SetBlock(x, y, z, new BlockGrass(), chunk);

                if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                {
                    for (int xi = -2; xi <= 2; xi++)
                    {
                        for (int zi = -2; zi <= 2; zi++)
                        {
                            SetBlock(x + xi, y, z + zi, new BlockAutumnGrass(), chunk, true);
                        }
                    }
                    CreateTree(x, y + 1, z, chunk);
                }

                if (y == dirtHeight && GetNoise(x, 0, z, grassFrequency, 100) < grassDensity)
                {
                    SetBlock(x, y + 1, z, new BlockTallGrass(), chunk);
                }
                    
            }
            

            else
            {
                SetBlock(x, y, z, new BlockAir(), chunk);
            }

        }

        return chunk;
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }

    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;



        if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
        {
            if (replaceBlocks || chunk.blocks[x, y, z] == null)
                chunk.SetBlock(x, y, z, block);
        }
    }

    void CreateTree(int x, int y, int z, Chunk chunk)
    {

        int randXLow = Random.Range(1, 3);
        int randXHigh = Random.Range(1, 3);
        int randZLow = Random.Range(1, 3);
        int randZHigh = Random.Range(1, 3);
        int randYLow = Random.Range(3, 5);
        int randYHigh = Random.Range(7, 9);
        //create leaves
        for (int xi = -randXLow; xi <= randXHigh; xi++)
        {
            for (int yi = randYLow; yi <= randYHigh; yi++)
            {
                for (int zi = -randZLow; zi <= randZHigh; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                }
            }
        }

        //create trunk
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }

    }
}