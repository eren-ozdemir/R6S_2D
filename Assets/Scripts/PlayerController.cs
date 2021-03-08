using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float step;
    public int speed;
    public GameObject moveReference;
    Transform attackDirection;
    Player player;

    [Header("Reinforce")]
    public Tilemap softWalls;
    public Tilemap hardWalls;
    public TileBase tileBase;
    public float reinforceRange;

    private void Start()
    {
        player = transform.GetComponent<Player>();
        attackDirection = GetComponent<PlayerCombat>().attackDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            Move(0, step);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            Move(-step, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, -90f);
            Move(0, -step);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            attackDirection.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Move(step, 0);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Reinforce(LookingTile());
        }
        else if (Input.GetMouseButtonDown(0))
        {
            LookingTile();

        }
        Vector3 mouseWorldPosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawRay(transform.position, mouseWorldPosition - transform.position, Color.red);
        
    }
    void Move(float horizontal, float vertical)
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(horizontal, vertical, 0), speed * Time.deltaTime);
    }


    Vector3Int LookingTile()
    {
        //Layer numbers
        int path = 8;
        int attack = 9;
        //int defense = 10;
        //int softWalls = 11;
        //int hardWalls = 12;

        //Layer mask for ray (Ray will interact everything except friendly and path layers)
        int bitmask = ~(1 << attack) & ~(1 << path);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, mouseWorldPosition - transform.position, player.designRange, bitmask);
        if (hitInfo)
        {
            Tilemap hittedTileMap = hitInfo.transform.GetComponent<Tilemap>();

            if (hittedTileMap)
            {
                Vector3Int lookingTile = hittedTileMap.WorldToCell(hitInfo.point);
                return lookingTile;
            }
        }
        return new Vector3Int(0,0,0);

    }

    void Reinforce(Vector3Int lookingTile)
    {
        float distance = Vector3.Distance(transform.position, lookingTile);
        if(distance <= reinforceRange)
        {
            hardWalls.SetTile(lookingTile, tileBase);
            softWalls.SetTile(lookingTile, null);
        }
    }
}
