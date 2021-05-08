using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct GridPoint
{
    public Vector3 pos;
    public bool active;
    public bool peaked;
    public Vector2Int chunk;
}

public struct Chunk
{
    public List<Vector3> verts;
    public List<Vector2> uvs;
    public List<int> tri;
    public Mesh mesh;
    public Vector2Int chunk;


}

