using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float step;
    public int speed;
    public int moveRange;
    public Vector3 movePosition;
    public Tilemap stops;
    public float maxX = .5f;
    public GameObject moveReference;
    public Transform attackDirection;
    Player player;

    private void Start()
    {
        player = transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            Move(0, step);
        }else if(Input.GetKeyDown(KeyCode.A))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            Move(-step, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, -90f);
            Move(0, -step);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Move(step, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(movePosition, 0.2f);
    }

    void Move(float horizontal, float vertical)
    {
        movePosition = moveReference.transform.position + new Vector3(horizontal, vertical, 0);
        Vector3Int nextTile = stops.WorldToCell(movePosition);

        for (int i = 0; i < moveRange; i++)
        {
            if (stops.GetTile(nextTile) == null)
            {
                movePosition += new Vector3(horizontal, vertical, 0);
                nextTile = stops.WorldToCell(movePosition);
            }
            else
            {
                transform.position = movePosition;
            }
        }
    }

}
