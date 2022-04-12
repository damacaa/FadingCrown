using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    SpriteRenderer sprite;
    Animator animator;

    public AudioClip jump;
    public AudioClip dash;
    public AudioClip slash;
    public AudioClip getDamage;
    public AudioClip die;
    public AudioClip land;


    AudioSource audio;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

    }

    public void SetFlip(bool flip)
    {
        sprite.flipX = flip;
    }

    public void SetCanMove(bool canMove)
    {
        if (animator)
            animator.SetBool("CanMove", canMove);
    }

    public void Jump()
    {
        if (jump != null)
        {
            audio.PlayOneShot(jump);
        }

        if (animator)
            animator.SetTrigger("Jump");
    }

    public void Attack()
    {
        if (slash != null)
        {
            audio.PlayOneShot(slash);
        }

        if (animator)
            animator.SetTrigger("Attack");
    }

    public void Dash()
    {
        if (dash != null)
        {
            audio.PlayOneShot(dash);
        }
        if (animator)
            animator.SetTrigger("Dash");
    }

    public void StopDash()
    {
        if (animator)
            animator.SetTrigger("StopDash");
    }

    bool previousOnGround = false;
    public void SetGround(bool onGround)
    {
        if (previousOnGround && onGround && land != null)
        {
            audio.PlayOneShot(land);
        }
        previousOnGround = onGround;


        if (animator)
            animator.SetBool("OnGround", onGround);
    }

    public void SetSpeedX(float speedX)
    {
        if (animator)
            animator.SetFloat("SpeedX", speedX);
    }

    internal void Die()
    {
        if (die != null)
        {
            audio.PlayOneShot(die);
        }

        if (animator)
            animator.SetTrigger("Die");
    }
}
