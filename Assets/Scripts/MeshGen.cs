using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject spheresActive;
    public GameObject spheresInactive;
    void Start()
    {
        Vector3[] Corners = new Vector3[8]
             {
             new Vector3(0, 0, 0),
             new Vector3(1, 0, 0),
             new Vector3(1, 0, 1),
             new Vector3(0, 0, 1),
             new Vector3(0, 1, 0),
             new Vector3(1, 1, 0),
             new Vector3(1, 1, 1),
             new Vector3(0, 1, 1)
             };

        List<Vector2> edges = new List<Vector2>();

        edges.Add(new Vector2(0, 4));
        edges.Add(new Vector2(0, 1));
        edges.Add(new Vector2(1, 5));
        edges.Add(new Vector2(4, 5));

        edges.Add(new Vector2(2, 6));
        edges.Add(new Vector2(2, 3));
        edges.Add(new Vector2(3, 7));
        edges.Add(new Vector2(6, 7));

        edges.Add(new Vector2(0, 2));
        edges.Add(new Vector2(1, 3));
        edges.Add(new Vector2(5, 7));
        edges.Add(new Vector2(4, 6));
        

       // List<Vector3> verts = new List<Vector3>();
        Vector3[] vertices = new Vector3[12]
            { 
            (Corners[0]+ Corners[1])/2,
            (Corners[1]+ Corners[2])/2,
            (Corners[2]+ Corners[3])/2,
            (Corners[3]+ Corners[0])/2,

            (Corners[4]+ Corners[5])/2,
            (Corners[5]+ Corners[6])/2,
            (Corners[6]+ Corners[7])/2,
            (Corners[7]+ Corners[4])/2,

            (Corners[0]+ Corners[4])/2,
            (Corners[5]+ Corners[1])/2,
            (Corners[2]+ Corners[6])/2,
            (Corners[7]+ Corners[3])/2,
            };

        int[] tri = new int[9] 
        {
        5,0,1,
        5,4,0,
        0,0,0,

        };


        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = tri;

        
        GameObject gameobject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        //gameobject.transform.localScale = new Vector3(30, 30, 30);
        gameobject.GetComponent<MeshFilter>().mesh = mesh;


        Instantiate(spheresInactive, Corners[0], Quaternion.identity);
        Instantiate(spheresActive, Corners[1], Quaternion.identity);
        Instantiate(spheresInactive, Corners[2], Quaternion.identity);
        Instantiate(spheresInactive, Corners[3], Quaternion.identity);
        Instantiate(spheresInactive, Corners[4], Quaternion.identity);
        Instantiate(spheresActive, Corners[5], Quaternion.identity);
        Instantiate(spheresInactive, Corners[6], Quaternion.identity);
        Instantiate(spheresActive, Corners[7], Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
