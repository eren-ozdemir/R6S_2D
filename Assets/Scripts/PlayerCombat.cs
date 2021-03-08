using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public Transform attackDirection;
    Player player;
    Animator playerAnimator;

    private void Start()
    {
        player = GetComponent<Player>();
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
    public void Attack()
    {
        //Play Attack Animation
        
        playerAnimator.SetTrigger("Attack");

        //Detect Target Objects

        //Layer numbers
        int path = 8;
        int attack = 9;

        //Layer mask for ray (Ray will interact everything except friendly and path layers)
        int bitmask = ~(1 << attack) & ~(1 << path);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, mouseWorldPosition - transform.position, player.attackRange, bitmask);
        
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

        if (!hittedPlayer.isDead)
        {
            int damage = transform.GetComponent<Player>().damage;
            hittedPlayer.health -= damage;
            hittedPlayerAnimator.SetTrigger("TakeHit");

            if (hittedPlayer.health <= 0)
            {
                hittedPlayerAnimator.SetTrigger("Dying");
                StartCoroutine(DelayedDead(hittedPlayer, hittedPlayerAnimator.GetCurrentAnimatorStateInfo(0).length));
            }
        }
        
    }

    IEnumerator DelayedDead(Player hittedPlayer, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        hittedPlayer.isDead = true;
        Destroy(hittedPlayer.GetComponentInParent<BoxCollider2D>());
    }
}
