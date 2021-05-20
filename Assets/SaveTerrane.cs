using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveTerrane : MonoBehaviour
{
    public MeshGeneration meshGen;
    int CX;
    int CZ;
    int x;
    int y;
    int z;
    bool active;


    public void LoadFile()
    {
        meshGen = new MeshGeneration();
        meshGen.grid = new GridPoint[meshGen.MAPSIZE_X, meshGen.MAPSIZE_Y, meshGen.MAPSIZE_Z, meshGen.ChunckSizeX, meshGen.ChunckSizeZ];

    }

}
