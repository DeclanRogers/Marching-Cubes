using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    RaycastHit hit;
    RaycastHit[] chunkChecker;
    public MeshGeneration meshGen;
    public float radius = 3;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            print(Vector3.Distance(transform.position, Vector3.zero));
        }



        if (Input.GetKey(KeyCode.Mouse0))
        {


            if (Physics.Raycast(transform.position, transform.forward * 100, out hit))
            {
                if (hit.transform.tag == "Ground")
                {

                    Vector3 centre = hit.point;
                    chunkChecker = Physics.SphereCastAll(centre, radius+2, Vector3.forward, radius+2);

                    foreach (var item in chunkChecker)
                    {




                        Vector2Int chunk;
                        if (item.transform.tag == "Ground")
                        {
                            chunk = item.transform.gameObject.GetComponent<ChunkTracker>().chunkPos;




                            for (int y = 0; y < meshGen.MAPSIZE_Y; y++)
                            {
                                for (int x = 0; x < meshGen.MAPSIZE_X; x++)
                                {
                                    for (int z = 0; z < meshGen.MAPSIZE_Z; z++)
                                    {
                                        if (Mathf.Pow((meshGen.grid[x, y, z, chunk.x, chunk.y].pos.x - centre.x), 2) + Mathf.Pow((meshGen.grid[x, y, z, chunk.x, chunk.y].pos.y - centre.y), 2) + Mathf.Pow((meshGen.grid[x, y, z, chunk.x, chunk.y].pos.z - centre.z), 2) < Mathf.Pow(radius, 2))
                                        {
                                            meshGen.grid[x, y, z, chunk.x, chunk.y].active = false;

                                        }
                                    }
                                }
                            }
                            meshGen.MarchingCubesUpdate(chunk.x, chunk.y);

                            meshGen.chunk[chunk.x, chunk.y].mesh.vertices = meshGen.chunk[chunk.x, chunk.y].verts.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.uv = meshGen.chunk[chunk.x, chunk.y].uvs.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.triangles = meshGen.chunk[chunk.x, chunk.y].tri.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.RecalculateNormals();
                            item.transform.gameObject.GetComponent<MeshFilter>().mesh = null;
                            item.transform.gameObject.GetComponent<MeshFilter>().mesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                            item.transform.gameObject.GetComponent<MeshCollider>().sharedMesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                        }
                    }
                   print(chunkChecker.Length);
                }

            }

        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Physics.Raycast(transform.position, transform.forward * 100, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    Vector3 centre = hit.point;
                    Vector2Int chunk;

                    chunkChecker = Physics.SphereCastAll(centre, radius+4, Vector3.forward, radius+4);

                    foreach (var item in chunkChecker)
                    {
                        if (item.transform.tag == "Ground")
                        {
                            chunk = item.transform.gameObject.GetComponent<ChunkTracker>().chunkPos;

                            for (int y = 0; y < meshGen.MAPSIZE_Y; y++)
                            {
                                for (int x = 0; x < meshGen.MAPSIZE_X; x++)
                                {
                                    for (int z = 0; z < meshGen.MAPSIZE_Z; z++)
                                    {
                                        if (Mathf.Pow((meshGen.grid[x, y, z, chunk.x, chunk.y].pos.x - centre.x), 2) + Mathf.Pow((meshGen.grid[x, y, z, chunk.x, chunk.y].pos.y - centre.y), 2) + Mathf.Pow((meshGen.grid[x, y, z, chunk.x, chunk.y].pos.z - centre.z), 2) < Mathf.Pow(radius, 2))
                                        {
                                            meshGen.grid[x, y, z, chunk.x, chunk.y].active = true;

                                        }
                                    }
                                }
                            }
                            meshGen.MarchingCubesUpdate(chunk.x, chunk.y);

                            meshGen.chunk[chunk.x, chunk.y].mesh.vertices = meshGen.chunk[chunk.x, chunk.y].verts.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.uv = meshGen.chunk[chunk.x, chunk.y].uvs.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.triangles = meshGen.chunk[chunk.x, chunk.y].tri.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.RecalculateNormals();
                            item.transform.gameObject.GetComponent<MeshFilter>().mesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                            item.transform.gameObject.GetComponent<MeshCollider>().sharedMesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                        }
                    }
                    print(chunkChecker.Length);
                }
            }

        }
        if (Input.mouseScrollDelta.y != 0)
        {
            radius += Input.mouseScrollDelta.y / 10;


        }
    }
}
