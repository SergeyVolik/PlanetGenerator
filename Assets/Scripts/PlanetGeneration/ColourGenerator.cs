using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator 
{

    ColorSettings m_settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;
    public void UpdateSettings(ColorSettings settings)
    {
        m_settings = settings;
        if (texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution*2, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32, false);
        }

        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
    }

    public void UpdateEleveation(MinMax elevationMinMax)
    {
        m_settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitShare)
    {
        float heightPercent = (pointOnUnitShare.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitShare) - m_settings.biomeColourSettings.noiseOffset) * m_settings.biomeColourSettings.noiseStrenght;
        float biomeIndex = 0;
        int numBiomes = m_settings.biomeColourSettings.biomes.Length;
        float blendRange = m_settings.biomeColourSettings.blendAmount / 2f + 0.001f;

        for (var i = 0; i < numBiomes; i++)
        {
            float dst = heightPercent - m_settings.biomeColourSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, (numBiomes - 1));
    }
    public void UpdateColours()
    {
        Color[] colours = new Color[texture.width* texture.height];
        int colourIndex = 0; 
        foreach (var biom in m_settings.biomeColourSettings.biomes)
        {
            for (var i = 0; i < textureResolution * 2; i++)
            {

                Color gradientCol;
                if (i < textureResolution)
                {
                    gradientCol = m_settings.oceanColour.Evaluate(i / (textureResolution - 1f));
                }
                else {
                    gradientCol = biom.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                }
               
                Color tintColor = biom.tint;
                colours[colourIndex] = gradientCol * (1 - biom.tintPercent) + tintColor * biom.tintPercent;
                colourIndex++;
            }

            
        }
        texture.SetPixels(colours);
        texture.Apply();
        m_settings.planetMaterial.SetTexture("_texture", texture);
    }
}
