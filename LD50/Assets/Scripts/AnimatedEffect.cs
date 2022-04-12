using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEffect : MonoBehaviour
{
    [SerializeField]
    bool dealsDamage = false;
    [SerializeField]
    int damage = 1;
    [SerializeField]
    bool knocks = false;
    [SerializeField]
    float knockVelocity = 10f;
    [SerializeField]
    float knockTime = 1f;

    public CharacterController user;
    public List<CharacterController> targets = new List<CharacterController>();

    Collider2D col;

    private void Awake()
    {
        if (TryGetComponent<Collider2D>(out col))
            col.enabled = false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Flip()
    {
        GetComponent<SpriteRenderer>().flipX = true;
    }

    void ApplyEffect(CharacterController cc)
    {
        if (user == cc)
            return;

        if (dealsDamage)
        {
            cc.Hurt(damage);
        }

        if (knocks)
        {
            Vector2 dir = (cc.transform.position - transform.position);
            dir.y = Mathf.Abs(dir.y);
            dir.Normalize();

            cc.Knock(dir * knockVelocity, knockTime);
        }
    }

    #region events
    //Functions called from events in animation
    //Can either apply efect once in a specific frame or enable the collider so it affects everythin it touches while it's on
    public void Effect()
    {
        foreach (CharacterController t in targets)
        {
            ApplyEffect(t);
        }
    }
    public void EnableCollider()
    {
        if (col)
            col.enabled = true;
    }

    public void DisableCollider()
    {
        if (col)
            col.enabled = false;
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CharacterController>(out CharacterController cc))
        {
            ApplyEffect(cc);
        }
    }
}
