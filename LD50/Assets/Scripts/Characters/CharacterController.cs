using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AttackController))]
public abstract class CharacterController : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 1;
    public int health;
    [SerializeField]
    float invencibilityTime = 1f;
    float invencibilityEndTime = 0;
    public int direction = 1;

    public bool dead;
    protected bool attacking = false;

    protected MovementController movementController;
    protected AttackController attackController;
    protected CharacterAnimation characterAnimation;

    protected void Awake()
    {
        movementController = GetComponent<MovementController>();
        attackController = GetComponent<AttackController>();
        characterAnimation = GetComponent<CharacterAnimation>();

        health = maxHealth;
    }

    public void Move(float x)
    {
        if (dead)
            return;
        movementController.Move(x);
    }

    public void Move(float x, float y)
    {
        if (dead)
            return;
        movementController.Move(x, y);
    }

    internal void Knock(Vector2 velocity, float t)
    {
        if (dead)
            return;
        Debug.Log("knock " + name);
        movementController.Knock(velocity);
        StartCoroutine(RecoverAfet(t));
    }

    IEnumerator RecoverAfet(float t)
    {
        yield return new WaitForSeconds(t);
        if (!dead)
            movementController.Recover();
        yield return null;
    }


    //public void Move(float x, float y) { movementController.Move(x, y); }
    public void Jump()
    {
        if (dead)
            return;
        movementController.Jump();
    }
    public void Float()
    {
        if (dead)
            return;
        movementController.Float();
    }
    public void Attack()
    {
        if (dead)
            return;
        attackController.Attack();
    }
    public void Dash()
    {
        if (dead)
            return;
        movementController.Dash();

    }
    public void Hurt(int damage, bool ignoreInvincibility = false)
    {
        movementController.audios.AudioSelect(SoundManager.audioType.Damage, movementController.selfAudio);

        if (!ignoreInvincibility && (dead || Time.time < invencibilityEndTime))
            return;

        health -= damage;
        if (health <= 0)
            Die();
        else
        {
            ShowDamage();
            invencibilityEndTime = Time.time + invencibilityTime;
        }
    }
    public void Heal(int amount)
    {
        if (dead)
            return;
        health += amount;
        health = Mathf.Min(health, maxHealth);
    }
    public void Down()
    {

    }
    protected virtual void Die()
    {
        if (!dead)
            characterAnimation.Die();

        dead = true;
    }

    public void SelfDestroy()
    {
        if (gameObject)
            Destroy(gameObject);
    }

    protected void ShowDamage()
    {
        StartCoroutine(ShowDamageCoroutine());
    }

    IEnumerator ShowDamageCoroutine()
    {
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        for (int i = 0; i < invencibilityTime * 10; i++)
        {
            s.enabled = !s.enabled;

            yield return new WaitForSeconds(0.1f);
        }
        s.enabled = true;
        yield return null;
    }
}
