using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData 
{
    public Vector3 Up = Vector3.up;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;

    private int vertexIndex;
    private int triangleIndex;

    private float Width;
    private List<Vector3> Line;

    public MeshData(List<Vector3> line,float width)
    {
        Line = line;
        Width = width;

        vertices = new Vector3[Line.Count * 2];
        uvs = new Vector2[Line.Count * 2];
        triangles = new int[(Line.Count - 1) * 6];
        vertexIndex = triangleIndex = 0;

        int length = Line.Count;
        for(int i = 0; i < length; i++)
        {
            vertices[vertexIndex] = Line[i] + Up * Width;
            vertices[vertexIndex + length] = Line[i] - Up * Width;

            uvs[vertexIndex] = new Vector2(i / (float)length, 0);
            uvs[vertexIndex + length] = new Vector2(i / (float)length, 1);

            if (i < length - 1)
            {
                AddTriangle(vertexIndex, vertexIndex + length + 1, vertexIndex + length);
                AddTriangle(vertexIndex + length + 1, vertexIndex, vertexIndex + 1);
            }
            vertexIndex++;
        }
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }

    void AddTriangle(int a,int b,int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
}
