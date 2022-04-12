using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    float direction;
    public float force;
    public float randForceRange = .1f;
    Rigidbody2D rb;
    public int damage;
    Player player;

    [SerializeField]
    Vector3 fixedDirection;


    // Start is called before the first frame update
    void Start()
    {
        /*direction = transform.TransformDirection(new Vector3(transform.localPosition.x + 1, transform.localPosition.y + 1));
        direction = (Vector2)transform.position - direction;*/
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        direction = player.transform.position.x - transform.position.x;
        direction = direction / Mathf.Abs(direction);
            

        rb = GetComponent<Rigidbody2D>();

        fixedDirection.x = Mathf.Abs(fixedDirection.x) * direction;
        var apliedForce = force + Random.Range(-randForceRange, randForceRange);
        rb.AddForce(fixedDirection.normalized * apliedForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Estoy chocando contra " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hurt(damage);
        }

        this.gameObject.SetActive(false);
        Object.Destroy(this.gameObject);
    }

}
