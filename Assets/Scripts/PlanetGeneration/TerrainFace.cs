using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh m_mesh;
    int m_resolution;
    Vector3 m_localUp;
    Vector3 axisA;
    Vector3 axisB;
    ShapeGenerator m_shapeGenerator;
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        m_shapeGenerator = shapeGenerator;
        m_mesh = mesh;
        m_resolution = resolution;
        m_localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vectices = new Vector3[m_resolution * m_resolution];

        int[] triangles = new int[(m_resolution - 1) * (m_resolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = (m_mesh.uv.Length == vectices.Length) ?  m_mesh.uv : new Vector2[vectices.Length];
        for (var y = 0; y < m_resolution; y++)
        {
            for (var x = 0; x < m_resolution; x++)
            {
                int i = x + y * m_resolution;
                Vector3 percent = new Vector2(x, y) / (m_resolution - 1);

                Vector3 pointOnUnitCube = m_localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitShare = pointOnUnitCube.normalized;
                float unscaledElevation = m_shapeGenerator.CalculateUnsacaleElevation(pointOnUnitShare);
                vectices[i] = pointOnUnitShare * m_shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;
                if (x != m_resolution - 1 && y != m_resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + m_resolution + 1;
                    triangles[triIndex + 2] = i + m_resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + m_resolution + 1;

                    triIndex += 6;
                }
            }
        }

        m_mesh.Clear();
        m_mesh.vertices = vectices;
        m_mesh.triangles = triangles;
        m_mesh.RecalculateNormals();
        m_mesh.uv = uv;
    }

    public void UpdateUVs(ColourGenerator colourGenerator)
    {
        Vector2[] uv = new Vector2[m_resolution * m_resolution];

        for (var y = 0; y < m_resolution; y++)
        {
            for (var x = 0; x < m_resolution; x++)
            {
                int i = x + y * m_resolution;
                Vector3 percent = new Vector2(x, y) / (m_resolution - 1);

                Vector3 pointOnUnitCube = m_localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitShare = pointOnUnitCube.normalized;

                uv[i].x = colourGenerator.BiomePercentFromPoint(pointOnUnitShare);
            }
        }
        m_mesh.uv = uv;
    }
}
