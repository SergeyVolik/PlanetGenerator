using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;

    public override void OnInspectorGUI()
    {
      
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
                planet.GeneratePlanet();
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }
        DrawSettingEditor(planet.shapeSettings, planet.OnShapeSettingUpdate, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingEditor(planet.colorSettings, planet.OnColorSettingUpdate, ref planet.colorSettingsFoldout, ref colourEditor); 
    }


    void DrawSettingEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {


                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                        onSettingsUpdated?.Invoke();
                }
            }
        }
    }
    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
