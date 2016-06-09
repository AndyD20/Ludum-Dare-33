using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockTallGrass : Block
{

    public BlockTallGrass()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 3;
                tile.y = 3;
                return tile;
            case Direction.down:
                tile.x = 3;
                tile.y = 3;
                return tile;
            case Direction.south:
                tile.x = 3;
                tile.y = 2;
                return tile;
            case Direction.west:
                tile.x = 3;
                tile.y = 2;
                return tile;
        }

        tile.x = 3;
        tile.y = 1;

        return tile;
    }

    public override bool IsSolid(Direction direction)
    {
        return false;
    }

    public override MeshData Blockdata
     (Chunk chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.useRenderDataForCol = false;

        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up))
        {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south))
        {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north))
        {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west))
        {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east))
        {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }

        return meshData;

    }


    protected override MeshData FaceDataNorth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }


    protected override MeshData FaceDataSouth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }


    protected override MeshData FaceDataEast
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }


    protected override MeshData FaceDataWest
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.west));
        return meshData;
    }


}
