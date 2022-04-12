using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPlatform : MonoBehaviour
{
    public FlowerTypes type;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("SOMETHING ENTERED");
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.FlowerAchived(type);
        }
    }
}
