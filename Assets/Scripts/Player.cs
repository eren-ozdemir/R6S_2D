using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public Vector3Int onTile;
    public Vector3Int lookingTile;
    public int damage;
    public int attackRange;
    public bool isDead = false;
}
