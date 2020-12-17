using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.RigidNoiseSettings m_noiseSettings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings noiseSettings)
    {
        m_noiseSettings = noiseSettings;
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequancy = m_noiseSettings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (var i = 0; i < m_noiseSettings.numberLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequancy + m_noiseSettings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * m_noiseSettings.wightMultipliyer);
            noiseValue += v * amplitude;
            frequancy *= m_noiseSettings.roughness;
            amplitude *= m_noiseSettings.persistence;
        }

        noiseValue = noiseValue - m_noiseSettings.minValue;
        return noiseValue * m_noiseSettings.strenght;
    }
}
