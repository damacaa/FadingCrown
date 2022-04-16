using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : CharacterController
{
    [SerializeField]
    bool dumb = false;
    [SerializeField]
    float lifeSpan = 10f;
    float timeOfDeath = 0;
    [SerializeField]
    private int contactDamage = 1;

    [SerializeField]
    float maxDistanceFromStart = 10;
    [SerializeField]
    float visionRange = 5;
    [SerializeField]
    float attackRange = 1;
    [SerializeField]
    Vector2 attackCenter;

    [SerializeField]
    float updateRate = 0.2f;
    float lastUpdateTime = 0;

    Vector2 initialPos;
    Vector2 velocity = Vector3.zero;

    Collider2D col;

    private void Start()
    {
        initialPos = transform.position;
        timeOfDeath = Time.time + lifeSpan;

        //if (!GetComponent<Collider2D>().isTrigger)
        if (TryGetComponent<Collider2D>(out col))
            Physics2D.IgnoreCollision(GameManager.Player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private void FixedUpdate()
    {
        if (dead)
            return;

        Vector2 targetPos = GameManager.Player.transform.position;
        float distanceToTarget = Vector2.Distance(targetPos, transform.TransformPoint((Vector3)attackCenter));

        if (distanceToTarget < attackRange)
        {
            GameManager.Player.Hurt(contactDamage);
        }

        if (dumb)
        {
            Move(direction, Mathf.Cos(2f * Time.time) / 2f);
            if (Time.time > timeOfDeath)
                Die();
            return;
        }

        if (Time.time < lastUpdateTime + updateRate)
        {
            Move(velocity.x, velocity.y);
            return;
        }

        bool targetDetected = distanceToTarget <= visionRange;


        if (distanceToTarget > maxDistanceFromStart || !targetDetected || GameManager.Player.dead)
        {
            //Go back to initial position
            if (movementController.canFly)
            {
                velocity = 0.5f * (initialPos - (Vector2)transform.position).normalized;
            }
            else
                velocity.x = (initialPos.x - transform.position.x) > 0 ? 1 : -1;
        }
        else if (targetDetected)
        {
            if (distanceToTarget <= attackRange)
            {

            }

            //Follow player
            if (movementController.canFly)
            {
                velocity = (targetPos - (Vector2)transform.position).normalized;
            }
            else
                velocity.x = (targetPos.x - transform.position.x) > 0 ? 1 : -1;
        }

        velocity.Normalize();

        if (velocity.y == 0)
            Move(velocity.x);
        else
            Move(velocity.x, velocity.y);

        lastUpdateTime = Time.time;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Application.isEditor)
            initialPos = transform.position;

        Gizmos.color = new Color(0, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint((Vector3)attackCenter), attackRange);
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, visionRange);
        Gizmos.color = new Color(0, 0, 1, 0.15f);
        Gizmos.DrawWireSphere(initialPos, maxDistanceFromStart);
        //Gizmos.DrawSphere(initialPos, maxDistanceFromStart);
    }
#endif


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.Hurt(contactDamage);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.Hurt(contactDamage);
        }
    }


}
