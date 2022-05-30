using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public enum Type
    {
        Line,
        Sphere,
        Cube
    }

    public Type type;
    public Color color;
    public float size = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    void OnDrawGizmosSelected()
    {
        if (type == Type.Line)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * size);
        }
        else if (type == Type.Sphere)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.transform.position, size);
        }
        else if (type == Type.Cube)
        {
            Gizmos.color = color;
            Gizmos.DrawCube(transform.transform.position, Vector2.one * size);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
