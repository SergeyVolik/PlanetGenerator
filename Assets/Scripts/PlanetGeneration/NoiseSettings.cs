using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Ridgid };
    public FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings sipmleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
       
        public float strenght = 1;
        [Range(1, 8)]
        public int numberLayers = 1;
        public float roughness = 2;
        public float persistence = 0.5f;
        public float baseRoughness = 1;
        public Vector3 center;
        public float minValue;
    }
    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float wightMultipliyer = 0.8f;
    }

   
}
