using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.SimpleNoiseSettings m_noiseSettings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings noiseSettings)
    {
        m_noiseSettings = noiseSettings;
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequancy = m_noiseSettings.baseRoughness;
        float amplitude = 1;

        for (var i = 0; i < m_noiseSettings.numberLayers; i++)
        {
            float v = noise.Evaluate(point * frequancy + m_noiseSettings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequancy *= m_noiseSettings.roughness;
            amplitude *= m_noiseSettings.persistence;
        }

        noiseValue = noiseValue - m_noiseSettings.minValue;
        return noiseValue * m_noiseSettings.strenght;
    }
}
