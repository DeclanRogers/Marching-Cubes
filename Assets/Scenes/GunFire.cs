using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    RaycastHit hit;
    Collider[] chunkChecker;
    public MeshGeneration meshGen;
    public LayerMask lm = 5;
    public LayerMask lmi = 0;
    public float radius = 3;
    public GameObject sphere;
    GameObject point;

    private void Start()
    {
        point = Instantiate(sphere, Vector3.zero, Quaternion.identity);
        point.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
    }
    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 50, lmi);
        point.transform.position = hit.point;

        if (Input.GetKey(KeyCode.Mouse0))
        {


            if (hit.collider)
            {
                if (hit.transform.tag == "Ground")
                {

                    Vector3 centre = hit.point;
                    chunkChecker = Physics.OverlapBox(centre, new Vector3(radius + 2, 50, radius + 2), Quaternion.identity, lm);

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
                                        else
                                        {
                                            continue;
                                        }
                                    
                                    }
                                }
                            }
                            item.transform.parent.gameObject.isStatic = false;
                            meshGen.MarchingCubesUpdate(chunk.x, chunk.y);

                            meshGen.chunk[chunk.x, chunk.y].mesh.vertices = meshGen.chunk[chunk.x, chunk.y].verts.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.uv = meshGen.chunk[chunk.x, chunk.y].uvs.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.triangles = meshGen.chunk[chunk.x, chunk.y].tri.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.RecalculateNormals();
                            item.GetComponentInParent<MeshFilter>().mesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                            item.GetComponentInParent<MeshCollider>().sharedMesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                            item.transform.parent.gameObject.isStatic = true;
                        }
                    }
                   
                }

            }

        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (hit.collider)
            {
                if (hit.transform.tag == "Ground")
                {
                    Vector3 centre = hit.point;
                    Vector2Int chunk;

                    //chunkChecker = Physics.OverlapBox(centre, new Vector3(radius + 2, 50, radius + 2), Quaternion.identity, lm);
                    chunkChecker = Physics.OverlapSphere(centre, radius+4, lm);

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
                            item.transform.parent.gameObject.isStatic = false;
                            
                            meshGen.MarchingCubesUpdate(chunk.x, chunk.y);

                            meshGen.chunk[chunk.x, chunk.y].mesh.vertices = meshGen.chunk[chunk.x, chunk.y].verts.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.uv = meshGen.chunk[chunk.x, chunk.y].uvs.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.triangles = meshGen.chunk[chunk.x, chunk.y].tri.ToArray();
                            meshGen.chunk[chunk.x, chunk.y].mesh.RecalculateNormals();
                            item.GetComponentInParent<MeshFilter>().mesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                            item.GetComponentInParent<MeshCollider>().sharedMesh = meshGen.chunk[chunk.x, chunk.y].mesh;
                            item.transform.parent.gameObject.isStatic = true;
                        }
                    }
                    //  print(chunkChecker.Length);
                }
            }

        }
        if (Input.mouseScrollDelta.y != 0)
        {
            radius += Input.mouseScrollDelta.y / 10;
            point.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);


        }
    }
}
