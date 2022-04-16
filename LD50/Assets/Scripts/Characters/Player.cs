using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterController
{
    private void Awake()
    {
        base.Awake();
        GameManager.SetPlayer(this);
    }


    protected override void Die()
    {
        base.Die();
        GameManager.GameOver();
    }

    private void Update()
    {

    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Hurt(1);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Hurt(1);
        }
    }
}
