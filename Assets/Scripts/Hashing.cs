using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hashing : MonoBehaviour
{
    List<Vert>[] vert = new List<Vert>[25];
    public struct Vert
    {
        public Vector3 pos;
        public int tri;
    }

    public void hashInit()
    {
        for (int i = 0; i < 25; i++)
        {
            vert[i] = new List<Vert>();

        }
        
    }
    public int CheckAganstHash(Vector3 vertPos, int pottentialTri)
    {
        bool matched = false;
        int index = pottentialTri;
        int key = Mathf.FloorToInt(((vertPos.x * vertPos.x) / 2.5f) * 3 + ((vertPos.y * vertPos.y) / 2.5f) * 3 + ((vertPos.z * vertPos.z) / 2.5f) * 3);


        if (vert[key % 25].Count != 0)
        {
            foreach (var item in vert[key % 24])
            {
                if (item.pos == vertPos)
                {
                    index = item.tri;
                    matched = true;
                    return index;
                }
            }
            if (matched)
            {
                return index;
            }
        }
        Vert v = new Vert();
        v.pos = vertPos;
        v.tri = index;
        vert[key % 24].Add(v);
        return index;

    }
}
