using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    // Start is called before the first frame update
    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    public float fov = 180f;
    float viewDistance = 10f;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Vector3 origin = Vector3.zero;
    }
    private void LateUpdate() { 
        int rayCount = 70;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2d = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if(raycastHit2d.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = raycastHit2d.point;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {

                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000);
    }

    public void AimDownSight(bool aimDownSight)
    {
        if (aimDownSight)
        {
            viewDistance = 15f;
            fov = 45f;
        }
        else
        {
            viewDistance = 10f;
            fov = 60f;
        }
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov /2 ;
    }




}
