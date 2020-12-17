using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{

    public float planetRadius;
    public NoiseLayer[] noiseLeyers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFirstLayersAsMask;
        public NoiseSettings noiseSettings;
    }
}
