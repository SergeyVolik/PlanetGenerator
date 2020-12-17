using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.Ridgid:
                return new RigidNoiseFilter(settings.rigidNoiseSettings);
               
            case NoiseSettings.FilterType.Simple:
                return new SimpleNoiseFilter(settings.sipmleNoiseSettings);

        }

        return null;
    }
}
