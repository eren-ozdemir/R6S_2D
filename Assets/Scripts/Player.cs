using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public int damage;
    public int attackRange;
    public float designRange;
    public bool isDead = false;
    public Vector3Int onTile;
    public Vector3Int lookingTile;
}
