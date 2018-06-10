using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMeshLengthWidth : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        Bounds bounds = mesh.bounds;
        int i = 0;
        while (i < uvs.Length)
        {
            uvs[i] = new Vector2(vertices[i].x / bounds.size.x, vertices[i].z / bounds.size.x);
            i++;
        }
        mesh.uv = uvs;
        Debug.Log(bounds.size.x + " + " + bounds.size.y + " + " + bounds.size.z);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
