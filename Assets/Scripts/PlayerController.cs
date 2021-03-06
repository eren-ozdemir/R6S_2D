﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    public float step;
    public int speed;
    public GameObject moveReference;
    public Rigidbody2D rb;
    Vector2 movement;
    Player player;
    bool aimDownSight = false;

    [Header("Reinforce")]
    public Tilemap softWalls;
    public Tilemap hardWalls;
    public TileBase tileBase;
    bool isTileSoftWall = false;

    private void Start()
    {
        player = transform.GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.G))
        {
            Reinforce(LookingTile());
        }
        else if (Input.GetMouseButtonDown(0))
        {
            LookingTile();

        }else if (Input.GetMouseButtonDown(1))
        {
            fieldOfView.AimDownSight(aimDownSight);
            aimDownSight = !aimDownSight;

        }
        //Vector3 mouseWorldPosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.DrawRay(transform.position, mouseWorldPosition - transform.position, Color.red);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection((mouseWorldPosition - transform.position).normalized);
    }

    private void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        
    }

    Vector3Int LookingTile()
    {
        //Layer numbers
        int attack = 9;
        int defense = 10;
        //int softWalls = 11;
        //int hardWalls = 12;

        //Layer mask for ray (Ray will interact everything except friendly and path layers)
        int bitmask = ~(1 << attack) & ~(1 << defense);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, mouseWorldPosition - transform.position, player.designRange, bitmask);
        if (hitInfo)
        {
            Tilemap hittedTileMap = hitInfo.transform.GetComponent<Tilemap>();

            if(hittedTileMap.name == "Soft Wall")
            {
                isTileSoftWall = true;
            }
            else
            {
                isTileSoftWall = false;
            }

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
        if(isTileSoftWall)
        {
            softWalls.SetTile(lookingTile, null);
            hardWalls.SetTile(lookingTile, tileBase);
        }


    }
}
