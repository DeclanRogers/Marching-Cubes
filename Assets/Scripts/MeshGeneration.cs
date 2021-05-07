﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    [HideInInspector]
    public int MAPSIZE_X = 25;
    [HideInInspector]
    public int MAPSIZE_Y = 150;
    [HideInInspector]
    public int MAPSIZE_Z = 25;


    public int ChunckSizeX = 3;
    public int ChunckSizeZ = 3;

    public float ScaleBias = 2;
    public int Oct = 7;
    public float Persistance = 3.0f;
    public float Lac = 0.7f;


    public GridPoint[,,,,] grid;

    public GameObject[,,] spheres;
    float[,] heightMap;
    public int seed = 1337;

    public GameObject sphere;
    public GameObject sphere1;
    public GameObject peak;
    public GameObject below;

    [HideInInspector]
    public List<Mesh> mesh = new List<Mesh>();

    Vector3[] Corners = new Vector3[8];
    [HideInInspector]
    public List<List<Vector3>> Verts = new List<List<Vector3>>();
    [HideInInspector]
    public List<List<int>> Tri = new List<List<int>>();
    [HideInInspector]
    public List<List<Vector2>> uv = new List<List<Vector2>>();
    public Material SurfaceMat;
    public bool HumanMade = true;

    /* private void Update()
     {
         if (Input.GetKeyDown(KeyCode.Space))
         {
             for (int x = 0; x < MAPSIZE_X; x++)
             {
                 for (int y = 0; y < MAPSIZE_Y; y++)
                 {
                     for (int z = 0; z < MAPSIZE_Z; z++)
                     {

                         if (spheres[x, y, z].GetComponent<TerrainPoints>().enabled)
                         {
                             grid[x, y, z].active = true;
                         }


                     }
                 }
             }
             MarchingCubes();
             Mesh mesh = new Mesh();
             mesh.Clear();
             mesh.vertices = Verts.ToArray();
             mesh.triangles = Tri.ToArray();
             mesh.uv = uv.ToArray();


             GameObject MC = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
             //gameobject.transform.localScale = new Vector3(30, 30, 30);
             MC.GetComponent<MeshRenderer>().material = SurfaceMat;
             MC.GetComponent<MeshRenderer>().receiveShadows = false;

             mesh.RecalculateNormals();
             MC.GetComponent<MeshFilter>().mesh = mesh;
         }


     }*/


    void Start()
    {
        grid = new GridPoint[MAPSIZE_X, MAPSIZE_Y, MAPSIZE_Z, ChunckSizeX, ChunckSizeZ];

        heightMap = new float[ChunckSizeX * MAPSIZE_X, ChunckSizeZ * MAPSIZE_Z];

        GenerateHeightMap(ref heightMap, ScaleBias, seed, Oct, Persistance, Lac, Vector2.zero);

        for (int CX = 0; CX < ChunckSizeX; CX++)
        {
            for (int CZ = 0; CZ < ChunckSizeZ; CZ++)
            {
                for (int y = 0; y < MAPSIZE_Y; y++)
                {
                    for (int x = 0; x < MAPSIZE_X; x++)
                    {
                        for (int z = 0; z < MAPSIZE_Z; z++)
                        {
                            grid[x, y, z, CX, CZ].pos = new Vector3((CX * 25) + x - CX, y, (CZ * 25) + z - CZ);
                            grid[x, y, z, CX, CZ].active = false;
                            grid[x, y, z, CX, CZ].peaked = false;

                            grid[x, y, z, CX, CZ].hm = new Vector3((CX * 25) + x, (CZ * 25) + z, heightMap[(CX * (25 - 1)) + x, (CZ * (25 - 1)) + z]);
                            if (y <= Mathf.Floor(heightMap[(CX * (25 - 1)) + x, (CZ * (25 - 1)) + z] * 10.9f) - 2)
                            {
                                grid[x, y, z, CX, CZ].active = true;
                                if (y == Mathf.Floor(heightMap[(CX * (25 - 1)) + x, (CZ * (25 - 1)) + z] * 10.0f) - 2)
                                {

                                    grid[x, y, z, CX, CZ].peaked = true;

                                }

                            }
                        }
                    }
                }
            }
        }
        // foreach (var item in grid)
        // {
        //     if (item.peaked)
        //     {
        //         Instantiate(sphere1, item.pos, Quaternion.identity).name = item.hm.ToString();
        //     }
        // }

        //print(grid[0, 5, 1, 1, 1].peaked);
        //Instantiate(sphere1, grid[0, 5, 1, 1, 1].pos, Quaternion.identity);
        //print(grid[0, 4, 1, 1, 1].peaked);
        //Instantiate(sphere, grid[0, 4, 1, 1, 1].pos, Quaternion.identity);
        if (!HumanMade)
        {

            MarchingCubes();

            int q = 0;

            foreach (var item in mesh)
            {
                item.vertices = Verts[q].ToArray();
                item.triangles = Tri[q].ToArray();
                item.uv = uv[q].ToArray();

                q++;

                GameObject MC = new GameObject("Mesh" + q, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

                MC.gameObject.tag = "Ground";
                MC.GetComponent<MeshRenderer>().material = SurfaceMat;
                MC.GetComponent<MeshRenderer>().receiveShadows = false;
                MC.GetComponent<MeshCollider>().sharedMesh = item;
                item.RecalculateNormals();
                MC.GetComponent<MeshFilter>().mesh = item;
            }
        }
    }


    private void OnDrawGizmos()
    {


        // for (int i = 0; i < 10; i++)
        // {
        //     if (grid[0, i, 1, 1, 1].active)
        //     {
        //         Gizmos.color = Color.blue;
        //         Gizmos.DrawCube(grid[0, i, 1, 1, 1].pos, new Vector3(0.4f, 0.4f, 0.4f));
        //     }
        //     else
        //     {
        //         Gizmos.color = Color.grey;
        //         Gizmos.DrawCube(grid[0, i, 1, 1, 1].pos, new Vector3(0.4f, 0.4f, 0.4f));
        //     }
        // }

       // foreach (var item in grid)
       // {
       //     if (item.peaked || item.active)
       //     {
       //         Gizmos.color = Color.green;
       //         Gizmos.DrawSphere(item.pos, 0.2f);
       //         // print(item.pos);
       //     }
       // }
    }
    void MarchingCubes()
    {

        for (int CX = 0; CX < ChunckSizeX; CX++)
        {
            for (int CZ = 0; CZ < ChunckSizeZ; CZ++)
            {
                List<Vector3> verts = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                List<int> tri = new List<int>();




                for (int y = 0; y < MAPSIZE_Y - 1; y++)
                {
                    for (int z = 0; z < MAPSIZE_Z - 1; z++)
                    {
                        for (int x = 0; x < MAPSIZE_X - 1; x++)
                        {
                            int triIndex = 0;


                            // if (grid[x, y, z, CX, CZ].peaked)
                            // {
                            //     Instantiate(sphere1, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                            // }

                            // if (x == 1 || x == 23 || x == 0)
                            // {
                            //     if (x == 0)
                            //     {
                            //         Instantiate(sphere1, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                            //     }
                            //     else
                            //     {
                            //         Instantiate(sphere, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                            //     }
                            //
                            // }


                            // if (z == 0)
                            // {
                            //     Instantiate(sphere1, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                            //
                            // }

                            if (grid[x, y, z, CX, CZ].active)
                            {
                                triIndex += 1;
                            }

                            if (grid[x + 1, y, z, CX, CZ].active)
                            {
                                triIndex += 2;
                            }

                            if (grid[x + 1, y, z + 1, CX, CZ].active)
                            {
                                triIndex += 4;
                            }

                            if (grid[x, y, z + 1, CX, CZ].active)
                            {
                                triIndex += 8;
                            }


                            if (grid[x, y + 1, z, CX, CZ].active)
                            {
                                triIndex += 16;
                            }

                            if (grid[x + 1, y + 1, z, CX, CZ].active)
                            {
                                triIndex += 32;
                            }

                            if (grid[x + 1, y + 1, z + 1, CX, CZ].active)
                            {
                                triIndex += 64;
                            }

                            if (grid[x, y + 1, z + 1, CX, CZ].active)
                            {
                                triIndex += 128;
                            }

                            if (triIndex != 0 && triIndex != 255)
                            {

                                Corners[0] = grid[x, y, z, CX, CZ].pos;
                                Corners[1] = grid[x + 1, y, z, CX, CZ].pos;
                                Corners[2] = grid[x + 1, y, z + 1, CX, CZ].pos;
                                Corners[3] = grid[x, y, z + 1, CX, CZ].pos;

                                Corners[4] = grid[x, y + 1, z, CX, CZ].pos;
                                Corners[5] = grid[x + 1, y + 1, z, CX, CZ].pos;
                                Corners[6] = grid[x + 1, y + 1, z + 1, CX, CZ].pos;
                                Corners[7] = grid[x, y + 1, z + 1, CX, CZ].pos;


                                verts.Add((Corners[0] + Corners[1]) / 2);
                                verts.Add((Corners[1] + Corners[2]) / 2);
                                verts.Add((Corners[2] + Corners[3]) / 2);
                                verts.Add((Corners[3] + Corners[0]) / 2);
                                verts.Add((Corners[4] + Corners[5]) / 2);
                                verts.Add((Corners[5] + Corners[6]) / 2);
                                verts.Add((Corners[6] + Corners[7]) / 2);
                                verts.Add((Corners[7] + Corners[4]) / 2);
                                verts.Add((Corners[0] + Corners[4]) / 2);
                                verts.Add((Corners[5] + Corners[1]) / 2);
                                verts.Add((Corners[2] + Corners[6]) / 2);
                                verts.Add((Corners[7] + Corners[3]) / 2);

                                // Verts.Add(qq);
                                // qq = new List<Vector3>();


                                uvs.Add(new Vector2(0, 1));
                                uvs.Add(new Vector2(1, 1));
                                uvs.Add(new Vector2(0, 0));
                                uvs.Add(new Vector2(1, 0));
                                uvs.Add(new Vector2(0, 1));
                                uvs.Add(new Vector2(1, 1));
                                uvs.Add(new Vector2(0, 0));
                                uvs.Add(new Vector2(1, 0));
                                uvs.Add(new Vector2(0, 1));
                                uvs.Add(new Vector2(1, 1));
                                uvs.Add(new Vector2(0, 0));
                                uvs.Add(new Vector2(1, 0));


                                // uv.Add(qqv);
                                // qqv = new List<Vector2>();

                                for (int i = 0; i < 15; i++)
                                {
                                    //print(triIndex);
                                    int valA = -1;
                                    if (verts.Count < 11)
                                    {

                                        valA = (TriangleTable[triIndex, i]);
                                    }
                                    else
                                    {

                                        valA = (TriangleTable[triIndex, i] + (verts.Count - 12));
                                    }

                                    if (valA != -1)
                                    {
                                        int index = 0;
                                        bool matched = false;
                                        foreach (var item in verts)
                                        {
                                            if (item == verts[valA])
                                            {
                                                tri.Add(index);
                                                matched = true;
                                                print(1);
                                                break;
                                            }
                                            index++;
                                        }
                                        if (!matched)
                                        { 
                                        tri.Add(valA);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                Tri.Add(tri);
                Verts.Add(verts);
                uv.Add(uvs);
                // tri = new List<int>();
                mesh.Add(new Mesh());
            }
        }
    }

    public Mesh MarchingCubesUpdate(int CX, int CZ)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tri = new List<int>();




        for (int y = 0; y < MAPSIZE_Y - 1; y++)
        {
            for (int z = 0; z < MAPSIZE_Z - 1; z++)
            {
                for (int x = 0; x < MAPSIZE_X - 1; x++)
                {
                    int triIndex = 0;


                    // if (grid[x, y, z, CX, CZ].peaked)
                    // {
                    //     Instantiate(sphere1, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                    // }

                    // if (x == 1 || x == 23 || x == 0)
                    // {
                    //     if (x == 0)
                    //     {
                    //         Instantiate(sphere1, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                    //     }
                    //     else
                    //     {
                    //         Instantiate(sphere, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                    //     }
                    //
                    // }


                    // if (z == 0)
                    // {
                    //     Instantiate(sphere1, grid[x, y, z, CX, CZ].pos, Quaternion.identity).name = grid[x, y, z, CX, CZ].hm.ToString();
                    //
                    // }

                    if (grid[x, y, z, CX, CZ].active)
                    {
                        triIndex += 1;
                    }

                    if (grid[x + 1, y, z, CX, CZ].active)
                    {
                        triIndex += 2;
                    }

                    if (grid[x + 1, y, z + 1, CX, CZ].active)
                    {
                        triIndex += 4;
                    }

                    if (grid[x, y, z + 1, CX, CZ].active)
                    {
                        triIndex += 8;
                    }


                    if (grid[x, y + 1, z, CX, CZ].active)
                    {
                        triIndex += 16;
                    }

                    if (grid[x + 1, y + 1, z, CX, CZ].active)
                    {
                        triIndex += 32;
                    }

                    if (grid[x + 1, y + 1, z + 1, CX, CZ].active)
                    {
                        triIndex += 64;
                    }

                    if (grid[x, y + 1, z + 1, CX, CZ].active)
                    {
                        triIndex += 128;
                    }

                    if (triIndex != 0 && triIndex != 255)
                    {

                        Corners[0] = grid[x, y, z, CX, CZ].pos;
                        Corners[1] = grid[x + 1, y, z, CX, CZ].pos;
                        Corners[2] = grid[x + 1, y, z + 1, CX, CZ].pos;
                        Corners[3] = grid[x, y, z + 1, CX, CZ].pos;

                        Corners[4] = grid[x, y + 1, z, CX, CZ].pos;
                        Corners[5] = grid[x + 1, y + 1, z, CX, CZ].pos;
                        Corners[6] = grid[x + 1, y + 1, z + 1, CX, CZ].pos;
                        Corners[7] = grid[x, y + 1, z + 1, CX, CZ].pos;


                        verts.Add((Corners[0] + Corners[1]) / 2);
                        verts.Add((Corners[1] + Corners[2]) / 2);
                        verts.Add((Corners[2] + Corners[3]) / 2);
                        verts.Add((Corners[3] + Corners[0]) / 2);
                        verts.Add((Corners[4] + Corners[5]) / 2);
                        verts.Add((Corners[5] + Corners[6]) / 2);
                        verts.Add((Corners[6] + Corners[7]) / 2);
                        verts.Add((Corners[7] + Corners[4]) / 2);
                        verts.Add((Corners[0] + Corners[4]) / 2);
                        verts.Add((Corners[5] + Corners[1]) / 2);
                        verts.Add((Corners[2] + Corners[6]) / 2);
                        verts.Add((Corners[7] + Corners[3]) / 2);

                        // Verts.Add(qq);
                        // qq = new List<Vector3>();


                        uvs.Add(new Vector2(0, 1));
                        uvs.Add(new Vector2(1, 1));
                        uvs.Add(new Vector2(0, 0));
                        uvs.Add(new Vector2(1, 0));
                        uvs.Add(new Vector2(0, 1));
                        uvs.Add(new Vector2(1, 1));
                        uvs.Add(new Vector2(0, 0));
                        uvs.Add(new Vector2(1, 0));
                        uvs.Add(new Vector2(0, 1));
                        uvs.Add(new Vector2(1, 1));
                        uvs.Add(new Vector2(0, 0));
                        uvs.Add(new Vector2(1, 0));


                        // uv.Add(qqv);
                        // qqv = new List<Vector2>();

                        for (int i = 0; i < 15; i++)
                        {
                            //print(triIndex);
                            int valA = -1;
                            if (verts.Count < 11)
                            {

                                valA = (TriangleTable[triIndex, i]);
                            }
                            else
                            {

                                valA = (TriangleTable[triIndex, i] + (verts.Count - 12));
                            }

                            if (valA != -1)
                            {
                                int index = 0;
                                bool matched = false;
                                foreach (var item in verts)
                                {
                                    if (item == verts[valA])
                                    {
                                        tri.Add(index);
                                        matched = true;
                                        print(1);
                                        break;
                                    }
                                    index++;
                                }
                                if (!matched)
                                {
                                    tri.Add(valA);
                                }

                            }
                        }
                    }
                }
            }
        }

        Tri[0].Clear();
        Tri[0] = tri;
        Verts[0].Clear();
        Verts[0] = verts;
        uv[0].Clear();
        uv[0] = uvs;
        // tri = new List<int>();
        return new Mesh();
    }





    private void GenerateHeightMap(ref float[,] heightMap, float scaleBias, int seed, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        System.Random pRandNumGen = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = pRandNumGen.Next(-100000, 100000) + offset.x;
            float offsetY = pRandNumGen.Next(-100000, 100000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scaleBias <= 0)
            scaleBias = 0.0001f;

        // Set min and max heights to max and min float values
        float minNoiseHeight = Mathf.Infinity;
        float maxNoiseHeight = Mathf.NegativeInfinity;

        float halfSize = MAPSIZE_X * ChunckSizeX / 2f;

        for (int y = 0; y < MAPSIZE_X * ChunckSizeX; y++)
        {
            for (int x = 0; x < MAPSIZE_X * ChunckSizeX; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; o++)
                {
                    float sampleX = (x - halfSize) / scaleBias * frequency + octaveOffsets[o].x;
                    float sampleY = (y - halfSize) / scaleBias * frequency + octaveOffsets[o].y;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // Updates the min and max noise height values
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                heightMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < MAPSIZE_X * ChunckSizeX; y++)
        {
            for (int x = 0; x < MAPSIZE_X * ChunckSizeX; x++)
            {
                // Basically a normalise function. Returns a value between 0 and 1
                heightMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heightMap[x, y] * 1.534f);
            }
        }
    }
    private int[,] TriangleTable = new int[,]
    {
        {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 1, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 8, 3, 9, 8, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 3, 1, 2, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {9, 2, 10, 0, 2, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {2, 8, 3, 2, 10, 8, 10, 9, 8, -1, -1, -1, -1, -1, -1, -1},
        {3, 11, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 11, 2, 8, 11, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 9, 0, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 11, 2, 1, 9, 11, 9, 8, 11, -1, -1, -1, -1, -1, -1, -1},
        {3, 10, 1, 11, 10, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 10, 1, 0, 8, 10, 8, 11, 10, -1, -1, -1, -1, -1, -1, -1},
        {3, 9, 0, 3, 11, 9, 11, 10, 9, -1, -1, -1, -1, -1, -1, -1},
        {9, 8, 10, 10, 8, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 7, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 3, 0, 7, 3, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 1, 9, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 1, 9, 4, 7, 1, 7, 3, 1, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 10, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {3, 4, 7, 3, 0, 4, 1, 2, 10, -1, -1, -1, -1, -1, -1, -1},
        {9, 2, 10, 9, 0, 2, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1},
        {2, 10, 9, 2, 9, 7, 2, 7, 3, 7, 9, 4, -1, -1, -1, -1},
        {8, 4, 7, 3, 11, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {11, 4, 7, 11, 2, 4, 2, 0, 4, -1, -1, -1, -1, -1, -1, -1},
        {9, 0, 1, 8, 4, 7, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1},
        {4, 7, 11, 9, 4, 11, 9, 11, 2, 9, 2, 1, -1, -1, -1, -1},
        {3, 10, 1, 3, 11, 10, 7, 8, 4, -1, -1, -1, -1, -1, -1, -1},
        {1, 11, 10, 1, 4, 11, 1, 0, 4, 7, 11, 4, -1, -1, -1, -1},
        {4, 7, 8, 9, 0, 11, 9, 11, 10, 11, 0, 3, -1, -1, -1, -1},
        {4, 7, 11, 4, 11, 9, 9, 11, 10, -1, -1, -1, -1, -1, -1, -1},
        {9, 5, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {9, 5, 4, 0, 8, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 5, 4, 1, 5, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {8, 5, 4, 8, 3, 5, 3, 1, 5, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 10, 9, 5, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {3, 0, 8, 1, 2, 10, 4, 9, 5, -1, -1, -1, -1, -1, -1, -1},
        {5, 2, 10, 5, 4, 2, 4, 0, 2, -1, -1, -1, -1, -1, -1, -1},
        {2, 10, 5, 3, 2, 5, 3, 5, 4, 3, 4, 8, -1, -1, -1, -1},
        {9, 5, 4, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 11, 2, 0, 8, 11, 4, 9, 5, -1, -1, -1, -1, -1, -1, -1},
        {0, 5, 4, 0, 1, 5, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1},
        {2, 1, 5, 2, 5, 8, 2, 8, 11, 4, 8, 5, -1, -1, -1, -1},
        {10, 3, 11, 10, 1, 3, 9, 5, 4, -1, -1, -1, -1, -1, -1, -1},
        {4, 9, 5, 0, 8, 1, 8, 10, 1, 8, 11, 10, -1, -1, -1, -1},
        {5, 4, 0, 5, 0, 11, 5, 11, 10, 11, 0, 3, -1, -1, -1, -1},
        {5, 4, 8, 5, 8, 10, 10, 8, 11, -1, -1, -1, -1, -1, -1, -1},
        {9, 7, 8, 5, 7, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {9, 3, 0, 9, 5, 3, 5, 7, 3, -1, -1, -1, -1, -1, -1, -1},
        {0, 7, 8, 0, 1, 7, 1, 5, 7, -1, -1, -1, -1, -1, -1, -1},
        {1, 5, 3, 3, 5, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {9, 7, 8, 9, 5, 7, 10, 1, 2, -1, -1, -1, -1, -1, -1, -1},
        {10, 1, 2, 9, 5, 0, 5, 3, 0, 5, 7, 3, -1, -1, -1, -1},
        {8, 0, 2, 8, 2, 5, 8, 5, 7, 10, 5, 2, -1, -1, -1, -1},
        {2, 10, 5, 2, 5, 3, 3, 5, 7, -1, -1, -1, -1, -1, -1, -1},
        {7, 9, 5, 7, 8, 9, 3, 11, 2, -1, -1, -1, -1, -1, -1, -1},
        {9, 5, 7, 9, 7, 2, 9, 2, 0, 2, 7, 11, -1, -1, -1, -1},
        {2, 3, 11, 0, 1, 8, 1, 7, 8, 1, 5, 7, -1, -1, -1, -1},
        {11, 2, 1, 11, 1, 7, 7, 1, 5, -1, -1, -1, -1, -1, -1, -1},
        {9, 5, 8, 8, 5, 7, 10, 1, 3, 10, 3, 11, -1, -1, -1, -1},
        {5, 7, 0, 5, 0, 9, 7, 11, 0, 1, 0, 10, 11, 10, 0, -1},
        {11, 10, 0, 11, 0, 3, 10, 5, 0, 8, 0, 7, 5, 7, 0, -1},
        {11, 10, 5, 7, 11, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {10, 6, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 3, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {9, 0, 1, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 8, 3, 1, 9, 8, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1},
        {1, 6, 5, 2, 6, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 6, 5, 1, 2, 6, 3, 0, 8, -1, -1, -1, -1, -1, -1, -1},
        {9, 6, 5, 9, 0, 6, 0, 2, 6, -1, -1, -1, -1, -1, -1, -1},
        {5, 9, 8, 5, 8, 2, 5, 2, 6, 3, 2, 8, -1, -1, -1, -1},
        {2, 3, 11, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {11, 0, 8, 11, 2, 0, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1},
        {0, 1, 9, 2, 3, 11, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1},
        {5, 10, 6, 1, 9, 2, 9, 11, 2, 9, 8, 11, -1, -1, -1, -1},
        {6, 3, 11, 6, 5, 3, 5, 1, 3, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 11, 0, 11, 5, 0, 5, 1, 5, 11, 6, -1, -1, -1, -1},
        {3, 11, 6, 0, 3, 6, 0, 6, 5, 0, 5, 9, -1, -1, -1, -1},
        {6, 5, 9, 6, 9, 11, 11, 9, 8, -1, -1, -1, -1, -1, -1, -1},
        {5, 10, 6, 4, 7, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 3, 0, 4, 7, 3, 6, 5, 10, -1, -1, -1, -1, -1, -1, -1},
        {1, 9, 0, 5, 10, 6, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1},
        {10, 6, 5, 1, 9, 7, 1, 7, 3, 7, 9, 4, -1, -1, -1, -1},
        {6, 1, 2, 6, 5, 1, 4, 7, 8, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 5, 5, 2, 6, 3, 0, 4, 3, 4, 7, -1, -1, -1, -1},
        {8, 4, 7, 9, 0, 5, 0, 6, 5, 0, 2, 6, -1, -1, -1, -1},
        {7, 3, 9, 7, 9, 4, 3, 2, 9, 5, 9, 6, 2, 6, 9, -1},
        {3, 11, 2, 7, 8, 4, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1},
        {5, 10, 6, 4, 7, 2, 4, 2, 0, 2, 7, 11, -1, -1, -1, -1},
        {0, 1, 9, 4, 7, 8, 2, 3, 11, 5, 10, 6, -1, -1, -1, -1},
        {9, 2, 1, 9, 11, 2, 9, 4, 11, 7, 11, 4, 5, 10, 6, -1},
        {8, 4, 7, 3, 11, 5, 3, 5, 1, 5, 11, 6, -1, -1, -1, -1},
        {5, 1, 11, 5, 11, 6, 1, 0, 11, 7, 11, 4, 0, 4, 11, -1},
        {0, 5, 9, 0, 6, 5, 0, 3, 6, 11, 6, 3, 8, 4, 7, -1},
        {6, 5, 9, 6, 9, 11, 4, 7, 9, 7, 11, 9, -1, -1, -1, -1},
        {10, 4, 9, 6, 4, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 10, 6, 4, 9, 10, 0, 8, 3, -1, -1, -1, -1, -1, -1, -1},
        {10, 0, 1, 10, 6, 0, 6, 4, 0, -1, -1, -1, -1, -1, -1, -1},
        {8, 3, 1, 8, 1, 6, 8, 6, 4, 6, 1, 10, -1, -1, -1, -1},
        {1, 4, 9, 1, 2, 4, 2, 6, 4, -1, -1, -1, -1, -1, -1, -1},
        {3, 0, 8, 1, 2, 9, 2, 4, 9, 2, 6, 4, -1, -1, -1, -1},
        {0, 2, 4, 4, 2, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {8, 3, 2, 8, 2, 4, 4, 2, 6, -1, -1, -1, -1, -1, -1, -1},
        {10, 4, 9, 10, 6, 4, 11, 2, 3, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 2, 2, 8, 11, 4, 9, 10, 4, 10, 6, -1, -1, -1, -1},
        {3, 11, 2, 0, 1, 6, 0, 6, 4, 6, 1, 10, -1, -1, -1, -1},
        {6, 4, 1, 6, 1, 10, 4, 8, 1, 2, 1, 11, 8, 11, 1, -1},
        {9, 6, 4, 9, 3, 6, 9, 1, 3, 11, 6, 3, -1, -1, -1, -1},
        {8, 11, 1, 8, 1, 0, 11, 6, 1, 9, 1, 4, 6, 4, 1, -1},
        {3, 11, 6, 3, 6, 0, 0, 6, 4, -1, -1, -1, -1, -1, -1, -1},
        {6, 4, 8, 11, 6, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {7, 10, 6, 7, 8, 10, 8, 9, 10, -1, -1, -1, -1, -1, -1, -1},
        {0, 7, 3, 0, 10, 7, 0, 9, 10, 6, 7, 10, -1, -1, -1, -1},
        {10, 6, 7, 1, 10, 7, 1, 7, 8, 1, 8, 0, -1, -1, -1, -1},
        {10, 6, 7, 10, 7, 1, 1, 7, 3, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 6, 1, 6, 8, 1, 8, 9, 8, 6, 7, -1, -1, -1, -1},
        {2, 6, 9, 2, 9, 1, 6, 7, 9, 0, 9, 3, 7, 3, 9, -1},
        {7, 8, 0, 7, 0, 6, 6, 0, 2, -1, -1, -1, -1, -1, -1, -1},
        {7, 3, 2, 6, 7, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {2, 3, 11, 10, 6, 8, 10, 8, 9, 8, 6, 7, -1, -1, -1, -1},
        {2, 0, 7, 2, 7, 11, 0, 9, 7, 6, 7, 10, 9, 10, 7, -1},
        {1, 8, 0, 1, 7, 8, 1, 10, 7, 6, 7, 10, 2, 3, 11, -1},
        {11, 2, 1, 11, 1, 7, 10, 6, 1, 6, 7, 1, -1, -1, -1, -1},
        {8, 9, 6, 8, 6, 7, 9, 1, 6, 11, 6, 3, 1, 3, 6, -1},
        {0, 9, 1, 11, 6, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {7, 8, 0, 7, 0, 6, 3, 11, 0, 11, 6, 0, -1, -1, -1, -1},
        {7, 11, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {7, 6, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {3, 0, 8, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 1, 9, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {8, 1, 9, 8, 3, 1, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1},
        {10, 1, 2, 6, 11, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 10, 3, 0, 8, 6, 11, 7, -1, -1, -1, -1, -1, -1, -1},
        {2, 9, 0, 2, 10, 9, 6, 11, 7, -1, -1, -1, -1, -1, -1, -1},
        {6, 11, 7, 2, 10, 3, 10, 8, 3, 10, 9, 8, -1, -1, -1, -1},
        {7, 2, 3, 6, 2, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {7, 0, 8, 7, 6, 0, 6, 2, 0, -1, -1, -1, -1, -1, -1, -1},
        {2, 7, 6, 2, 3, 7, 0, 1, 9, -1, -1, -1, -1, -1, -1, -1},
        {1, 6, 2, 1, 8, 6, 1, 9, 8, 8, 7, 6, -1, -1, -1, -1},
        {10, 7, 6, 10, 1, 7, 1, 3, 7, -1, -1, -1, -1, -1, -1, -1},
        {10, 7, 6, 1, 7, 10, 1, 8, 7, 1, 0, 8, -1, -1, -1, -1},
        {0, 3, 7, 0, 7, 10, 0, 10, 9, 6, 10, 7, -1, -1, -1, -1},
        {7, 6, 10, 7, 10, 8, 8, 10, 9, -1, -1, -1, -1, -1, -1, -1},
        {6, 8, 4, 11, 8, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {3, 6, 11, 3, 0, 6, 0, 4, 6, -1, -1, -1, -1, -1, -1, -1},
        {8, 6, 11, 8, 4, 6, 9, 0, 1, -1, -1, -1, -1, -1, -1, -1},
        {9, 4, 6, 9, 6, 3, 9, 3, 1, 11, 3, 6, -1, -1, -1, -1},
        {6, 8, 4, 6, 11, 8, 2, 10, 1, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 10, 3, 0, 11, 0, 6, 11, 0, 4, 6, -1, -1, -1, -1},
        {4, 11, 8, 4, 6, 11, 0, 2, 9, 2, 10, 9, -1, -1, -1, -1},
        {10, 9, 3, 10, 3, 2, 9, 4, 3, 11, 3, 6, 4, 6, 3, -1},
        {8, 2, 3, 8, 4, 2, 4, 6, 2, -1, -1, -1, -1, -1, -1, -1},
        {0, 4, 2, 4, 6, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 9, 0, 2, 3, 4, 2, 4, 6, 4, 3, 8, -1, -1, -1, -1},
        {1, 9, 4, 1, 4, 2, 2, 4, 6, -1, -1, -1, -1, -1, -1, -1},
        {8, 1, 3, 8, 6, 1, 8, 4, 6, 6, 10, 1, -1, -1, -1, -1},
        {10, 1, 0, 10, 0, 6, 6, 0, 4, -1, -1, -1, -1, -1, -1, -1},
        {4, 6, 3, 4, 3, 8, 6, 10, 3, 0, 3, 9, 10, 9, 3, -1},
        {10, 9, 4, 6, 10, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 9, 5, 7, 6, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 3, 4, 9, 5, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1},
        {5, 0, 1, 5, 4, 0, 7, 6, 11, -1, -1, -1, -1, -1, -1, -1},
        {11, 7, 6, 8, 3, 4, 3, 5, 4, 3, 1, 5, -1, -1, -1, -1},
        {9, 5, 4, 10, 1, 2, 7, 6, 11, -1, -1, -1, -1, -1, -1, -1},
        {6, 11, 7, 1, 2, 10, 0, 8, 3, 4, 9, 5, -1, -1, -1, -1},
        {7, 6, 11, 5, 4, 10, 4, 2, 10, 4, 0, 2, -1, -1, -1, -1},
        {3, 4, 8, 3, 5, 4, 3, 2, 5, 10, 5, 2, 11, 7, 6, -1},
        {7, 2, 3, 7, 6, 2, 5, 4, 9, -1, -1, -1, -1, -1, -1, -1},
        {9, 5, 4, 0, 8, 6, 0, 6, 2, 6, 8, 7, -1, -1, -1, -1},
        {3, 6, 2, 3, 7, 6, 1, 5, 0, 5, 4, 0, -1, -1, -1, -1},
        {6, 2, 8, 6, 8, 7, 2, 1, 8, 4, 8, 5, 1, 5, 8, -1},
        {9, 5, 4, 10, 1, 6, 1, 7, 6, 1, 3, 7, -1, -1, -1, -1},
        {1, 6, 10, 1, 7, 6, 1, 0, 7, 8, 7, 0, 9, 5, 4, -1},
        {4, 0, 10, 4, 10, 5, 0, 3, 10, 6, 10, 7, 3, 7, 10, -1},
        {7, 6, 10, 7, 10, 8, 5, 4, 10, 4, 8, 10, -1, -1, -1, -1},
        {6, 9, 5, 6, 11, 9, 11, 8, 9, -1, -1, -1, -1, -1, -1, -1},
        {3, 6, 11, 0, 6, 3, 0, 5, 6, 0, 9, 5, -1, -1, -1, -1},
        {0, 11, 8, 0, 5, 11, 0, 1, 5, 5, 6, 11, -1, -1, -1, -1},
        {6, 11, 3, 6, 3, 5, 5, 3, 1, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 10, 9, 5, 11, 9, 11, 8, 11, 5, 6, -1, -1, -1, -1},
        {0, 11, 3, 0, 6, 11, 0, 9, 6, 5, 6, 9, 1, 2, 10, -1},
        {11, 8, 5, 11, 5, 6, 8, 0, 5, 10, 5, 2, 0, 2, 5, -1},
        {6, 11, 3, 6, 3, 5, 2, 10, 3, 10, 5, 3, -1, -1, -1, -1},
        {5, 8, 9, 5, 2, 8, 5, 6, 2, 3, 8, 2, -1, -1, -1, -1},
        {9, 5, 6, 9, 6, 0, 0, 6, 2, -1, -1, -1, -1, -1, -1, -1},
        {1, 5, 8, 1, 8, 0, 5, 6, 8, 3, 8, 2, 6, 2, 8, -1},
        {1, 5, 6, 2, 1, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 3, 6, 1, 6, 10, 3, 8, 6, 5, 6, 9, 8, 9, 6, -1},
        {10, 1, 0, 10, 0, 6, 9, 5, 0, 5, 6, 0, -1, -1, -1, -1},
        {0, 3, 8, 5, 6, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {10, 5, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {11, 5, 10, 7, 5, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {11, 5, 10, 11, 7, 5, 8, 3, 0, -1, -1, -1, -1, -1, -1, -1},
        {5, 11, 7, 5, 10, 11, 1, 9, 0, -1, -1, -1, -1, -1, -1, -1},
        {10, 7, 5, 10, 11, 7, 9, 8, 1, 8, 3, 1, -1, -1, -1, -1},
        {11, 1, 2, 11, 7, 1, 7, 5, 1, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 3, 1, 2, 7, 1, 7, 5, 7, 2, 11, -1, -1, -1, -1},
        {9, 7, 5, 9, 2, 7, 9, 0, 2, 2, 11, 7, -1, -1, -1, -1},
        {7, 5, 2, 7, 2, 11, 5, 9, 2, 3, 2, 8, 9, 8, 2, -1},
        {2, 5, 10, 2, 3, 5, 3, 7, 5, -1, -1, -1, -1, -1, -1, -1},
        {8, 2, 0, 8, 5, 2, 8, 7, 5, 10, 2, 5, -1, -1, -1, -1},
        {9, 0, 1, 5, 10, 3, 5, 3, 7, 3, 10, 2, -1, -1, -1, -1},
        {9, 8, 2, 9, 2, 1, 8, 7, 2, 10, 2, 5, 7, 5, 2, -1},
        {1, 3, 5, 3, 7, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 7, 0, 7, 1, 1, 7, 5, -1, -1, -1, -1, -1, -1, -1},
        {9, 0, 3, 9, 3, 5, 5, 3, 7, -1, -1, -1, -1, -1, -1, -1},
        {9, 8, 7, 5, 9, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {5, 8, 4, 5, 10, 8, 10, 11, 8, -1, -1, -1, -1, -1, -1, -1},
        {5, 0, 4, 5, 11, 0, 5, 10, 11, 11, 3, 0, -1, -1, -1, -1},
        {0, 1, 9, 8, 4, 10, 8, 10, 11, 10, 4, 5, -1, -1, -1, -1},
        {10, 11, 4, 10, 4, 5, 11, 3, 4, 9, 4, 1, 3, 1, 4, -1},
        {2, 5, 1, 2, 8, 5, 2, 11, 8, 4, 5, 8, -1, -1, -1, -1},
        {0, 4, 11, 0, 11, 3, 4, 5, 11, 2, 11, 1, 5, 1, 11, -1},
        {0, 2, 5, 0, 5, 9, 2, 11, 5, 4, 5, 8, 11, 8, 5, -1},
        {9, 4, 5, 2, 11, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {2, 5, 10, 3, 5, 2, 3, 4, 5, 3, 8, 4, -1, -1, -1, -1},
        {5, 10, 2, 5, 2, 4, 4, 2, 0, -1, -1, -1, -1, -1, -1, -1},
        {3, 10, 2, 3, 5, 10, 3, 8, 5, 4, 5, 8, 0, 1, 9, -1},
        {5, 10, 2, 5, 2, 4, 1, 9, 2, 9, 4, 2, -1, -1, -1, -1},
        {8, 4, 5, 8, 5, 3, 3, 5, 1, -1, -1, -1, -1, -1, -1, -1},
        {0, 4, 5, 1, 0, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {8, 4, 5, 8, 5, 3, 9, 0, 5, 0, 3, 5, -1, -1, -1, -1},
        {9, 4, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 11, 7, 4, 9, 11, 9, 10, 11, -1, -1, -1, -1, -1, -1, -1},
        {0, 8, 3, 4, 9, 7, 9, 11, 7, 9, 10, 11, -1, -1, -1, -1},
        {1, 10, 11, 1, 11, 4, 1, 4, 0, 7, 4, 11, -1, -1, -1, -1},
        {3, 1, 4, 3, 4, 8, 1, 10, 4, 7, 4, 11, 10, 11, 4, -1},
        {4, 11, 7, 9, 11, 4, 9, 2, 11, 9, 1, 2, -1, -1, -1, -1},
        {9, 7, 4, 9, 11, 7, 9, 1, 11, 2, 11, 1, 0, 8, 3, -1},
        {11, 7, 4, 11, 4, 2, 2, 4, 0, -1, -1, -1, -1, -1, -1, -1},
        {11, 7, 4, 11, 4, 2, 8, 3, 4, 3, 2, 4, -1, -1, -1, -1},
        {2, 9, 10, 2, 7, 9, 2, 3, 7, 7, 4, 9, -1, -1, -1, -1},
        {9, 10, 7, 9, 7, 4, 10, 2, 7, 8, 7, 0, 2, 0, 7, -1},
        {3, 7, 10, 3, 10, 2, 7, 4, 10, 1, 10, 0, 4, 0, 10, -1},
        {1, 10, 2, 8, 7, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 9, 1, 4, 1, 7, 7, 1, 3, -1, -1, -1, -1, -1, -1, -1},
        {4, 9, 1, 4, 1, 7, 0, 8, 1, 8, 7, 1, -1, -1, -1, -1},
        {4, 0, 3, 7, 4, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {4, 8, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {9, 10, 8, 10, 11, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {3, 0, 9, 3, 9, 11, 11, 9, 10, -1, -1, -1, -1, -1, -1, -1},
        {0, 1, 10, 0, 10, 8, 8, 10, 11, -1, -1, -1, -1, -1, -1, -1},
        {3, 1, 10, 11, 3, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 2, 11, 1, 11, 9, 9, 11, 8, -1, -1, -1, -1, -1, -1, -1},
        {3, 0, 9, 3, 9, 11, 1, 2, 9, 2, 11, 9, -1, -1, -1, -1},
        {0, 2, 11, 8, 0, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {3, 2, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {2, 3, 8, 2, 8, 10, 10, 8, 9, -1, -1, -1, -1, -1, -1, -1},
        {9, 10, 2, 0, 9, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {2, 3, 8, 2, 8, 10, 0, 1, 8, 1, 10, 8, -1, -1, -1, -1},
        {1, 10, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {1, 3, 8, 9, 1, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 9, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {0, 3, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}
    };

}
