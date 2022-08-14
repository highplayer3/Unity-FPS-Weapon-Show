using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Parabolicmotion : MonoBehaviour
{
    private Vector3 pos;
    [SerializeField]
    private int PointsCount = 100;
    [SerializeField]
    private float interval = 0.1f;
    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float width = 1f;
    
    public float Gravity = 9.8f;
    private bool isPointFinish = false;

    List<Vector3> Points = new List<Vector3>();
    private MeshFilter TrackRender;

    private void Start()
    {
        pos = transform.position;
        TrackRender = GetComponent<MeshFilter>();
        PointRecord();
    }
    private void Update()
    {
        if (isPointFinish)
        {
            MeshData data = new MeshData(Points, width);
            TrackRender.mesh = data.CreateMesh();
        }
        OnSceneGUI();
        
    }
    private void PointRecord()
    {
        if (isPointFinish == false)
        {
            for (int i = 0; i < PointsCount; i++)
            {
                Points.Add(pos);
                velocity += Vector3.down * Gravity * interval;
                pos += velocity * interval;
            }
            isPointFinish = true;
        }
        
    }
    void OnSceneGUI()
    {
        Handles.color = Color.green;
        for(int i = 0; i < Points.Count; i++)
        {
            Handles.SphereHandleCap(0, Points[i], Quaternion.identity, 1, EventType.Repaint);
        }
    }
}