
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    // Start is called before the first frame update
    
    Noise noise = new Noise();

    public float Evalauate(Vector3 point)
    {
        
        float noiseValue = (noise.Evaluate(point) + 1)*0.5f;
        return noiseValue;
    }
}

