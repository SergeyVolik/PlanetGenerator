using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings m_settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;
    public void UpdateSettings(ShapeSettings settings)
    {
        m_settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLeyers.Length];

        for (var i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(m_settings.noiseLeyers[i].noiseSettings);
        }
        elevationMinMax = new MinMax();
    }

    public float CalculateUnsacaleElevation(Vector3 pointOnUnitShare)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitShare);

            if (m_settings.noiseLeyers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (var i = 1; i < noiseFilters.Length; i++)
        {
            if (m_settings.noiseLeyers[i].enabled)
            {
                float mask = (m_settings.noiseLeyers[i].useFirstLayersAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitShare) * mask;
            }
        }

       
        elevationMinMax.AddValue(elevation);
        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0, unscaledElevation);
        elevation = m_settings.planetRadius * (1 + elevation);
        return elevation;
    }
}
