using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public Transform attackDirection;
    LayerMask layerMask;
    int range;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
    public void Attack()
    {
        range = transform.GetComponent<Player>().attackRange;
        //Play Attack Animation
        Animator playerAnimator = GetComponent<Animator>();
        playerAnimator.SetTrigger("Attack");

        //Detect Target Objects

        //Layer numbers
        int path = 8;
        int attack = 9;

        //Layer mask for ray (Ray will interact everything except friendly and path layers)
        int bitmask = ~(1 << attack) & ~(1 << path);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, attackDirection.transform.right, range, bitmask);
        
        if (hitInfo)
        {
            Damage(hitInfo);
            Debug.Log(hitInfo.transform.name);
        }
    }

    void Damage(RaycastHit2D hitInfo)
    {
        Player hittedPlayer = hitInfo.transform.GetComponent<Player>();
        Animator hittedPlayerAnimator = hitInfo.transform.GetComponent<Animator>();

        int damage = transform.GetComponent<Player>().damage;
        hittedPlayer.health -= damage;

        if (hittedPlayer.health <= 0)
        {
            hittedPlayerAnimator.SetTrigger("Dying");
        }
    }
}
