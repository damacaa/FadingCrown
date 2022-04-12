using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtCollider : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Estoy chocando contra " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hurt(damage, true);
        }

        this.gameObject.SetActive(false);
        Object.Destroy(this.gameObject);
    }
}
