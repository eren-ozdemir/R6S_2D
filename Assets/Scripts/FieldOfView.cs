using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float angle = 90;
    public float viewDistance = 2f;
    Vector2 direction;
    Vector3 middle;
    Vector3 leftRay;
    Vector3 rightRay;
    Vector3 right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetVectorFromAngle(angle);
    }

    private void OnDrawGizmos()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.DrawSphere(mouseWorldPosition, 0.1f);
        Gizmos.DrawRay(transform.position, leftRay- transform.position);
    }

    void GetVectorFromAngle(float angle)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        angle /= 2;
        angle *= Mathf.Deg2Rad;

        middle = mouseWorldPosition;
        right = new Vector3(transform.position.x + viewDistance, transform.position.y);
        float angleFromRight = Vector3.Angle(middle - transform.position, right - transform.position);

        angleFromRight *= Mathf.Deg2Rad;

        leftRay = Vector3.zero;
        rightRay = Vector3.zero;


        if (middle.y >= transform.position.y)
        {
            leftRay.x = transform.position.x + viewDistance * Mathf.Cos(angleFromRight + angle);
            leftRay.y = transform.position.y + viewDistance * Mathf.Sin(angleFromRight + angle);
            rightRay.x = transform.position.x + viewDistance * Mathf.Cos(angleFromRight - angle);
            rightRay.y = transform.position.y + viewDistance * Mathf.Sin(angleFromRight - angle);

        }
        else{
            leftRay.x = transform.position.x + viewDistance * Mathf.Cos(2*Mathf.PI- angleFromRight + angle);
            leftRay.y = transform.position.y + viewDistance * Mathf.Sin(2*Mathf.PI - angleFromRight + angle);
            rightRay.x = transform.position.x + viewDistance * Mathf.Cos(2 * Mathf.PI - angleFromRight - angle);
            rightRay.y = transform.position.y + viewDistance * Mathf.Sin(2 * Mathf.PI - angleFromRight - angle);
        }

        Debug.DrawRay(transform.position, middle - transform.position, Color.black);
        Debug.DrawRay(transform.position, leftRay - transform.position, Color.yellow);
        Debug.DrawRay(transform.position, rightRay - transform.position, Color.red);

    }
}
