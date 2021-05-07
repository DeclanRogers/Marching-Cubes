using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    RaycastHit hit;
    public MeshGeneration meshGen;
    public float radius = 3;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.forward * 100);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {


            if (Physics.Raycast(transform.position, transform.forward * 100, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    Vector3 centre = hit.point;

                    for (int y = 0; y < meshGen.MAPSIZE_Y; y++)
                    {
                        for (int x = 0; x < meshGen.MAPSIZE_X; x++)
                        {
                            for (int z = 0; z < meshGen.MAPSIZE_Z; z++)
                            {
                                if (Mathf.Pow((meshGen.grid[x, y, z, 0, 0].pos.x - centre.x), 2) + Mathf.Pow((meshGen.grid[x, y, z, 0, 0].pos.y - centre.y), 2) + Mathf.Pow((meshGen.grid[x, y, z, 0, 0].pos.z - centre.z), 2) < Mathf.Pow(radius, 2))
                                {
                                    meshGen.grid[x, y, z, 0, 0].active = false;

                                }
                            }
                        }
                    }
                meshGen.Verts[0].Clear();
                meshGen.Tri[0].Clear();
                meshGen.uv[0].Clear();
                meshGen.mesh[0] = meshGen.MarchingCubesUpdate(0, 0);
                meshGen.mesh[0].vertices = meshGen.Verts[0].ToArray();
                meshGen.mesh[0].uv = meshGen.uv[0].ToArray();
                meshGen.mesh[0].triangles = meshGen.Tri[0].ToArray();
                meshGen.mesh[0].RecalculateNormals();
                hit.transform.gameObject.GetComponent<MeshFilter>().mesh = meshGen.mesh[0];
                hit.transform.gameObject.GetComponent<MeshCollider>().sharedMesh = meshGen.mesh[0];
                }

            }



            //if (Input.GetKeyDown(KeyCode.Escape))
            //    Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Physics.Raycast(transform.position, transform.forward * 100, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    Vector3 centre = hit.point;

                    for (int y = 0; y < meshGen.MAPSIZE_Y; y++)
                    {
                        for (int x = 0; x < meshGen.MAPSIZE_X; x++)
                        {
                            for (int z = 0; z < meshGen.MAPSIZE_Z; z++)
                            {
                                if (Mathf.Pow((meshGen.grid[x, y, z, 0, 0].pos.x - centre.x), 2) + Mathf.Pow((meshGen.grid[x, y, z, 0, 0].pos.y - centre.y), 2) + Mathf.Pow((meshGen.grid[x, y, z, 0, 0].pos.z - centre.z), 2) < Mathf.Pow(radius, 2))
                                {
                                    meshGen.grid[x, y, z, 0, 0].active = true;

                                }
                            }
                        }
                    }
                meshGen.Verts[0].Clear();
                meshGen.Tri[0].Clear();
                meshGen.uv[0].Clear();
                meshGen.mesh[0] = meshGen.MarchingCubesUpdate(0, 0);
                meshGen.mesh[0].vertices = meshGen.Verts[0].ToArray();
                meshGen.mesh[0].uv = meshGen.uv[0].ToArray();
                meshGen.mesh[0].triangles = meshGen.Tri[0].ToArray();
                meshGen.mesh[0].RecalculateNormals();
                hit.transform.gameObject.GetComponent<MeshFilter>().mesh = meshGen.mesh[0];
                hit.transform.gameObject.GetComponent<MeshCollider>().sharedMesh = meshGen.mesh[0];
                }

            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            radius += Input.mouseScrollDelta.y / 10;


        }
    }
}
