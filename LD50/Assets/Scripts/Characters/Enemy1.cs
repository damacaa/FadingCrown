using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyOld : CharacterController
{
    public float rangeOfVision;
    public float rangeOfAttack;
    Vector3 iniPosition;
    Vector3 actualPos;
    Vector3 nextPosition;
    float distance;
    float distPlayer;
    Player player;
    bool playerDetected;
    public int damage;
    //Distancias de generacion de Wander
    public float distGen;
    public bool rangeAttack;
    [SerializeField]GameObject prefabBala;
    public GameObject spawner;

    public int timeToDie;

    float rotation;
    public float rotMax;
    public float rotMin;

    private float cooldownIniTime = -1;
    public float attackCooldown = 1;
    private bool cooldown = false;  

    public float raycastFloorCheckDistance = 2f;

    Collider2D collider;
    LayerMask groundLayerMask;
    public bool right;
    public bool pattern;
    public float movX;
    public float movY;

    private void Start()
    {        
        iniPosition = gameObject.transform.position;
        NewObjective();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //aqui hay que mirar todos los colliders del objeto y quedarte solo con el que no esta triggereado para poder evitar chocar con el
        collider = GetComponent<Collider2D>();
        if(!collider.isTrigger)
            Physics2D.IgnoreCollision(collider, player.gameObject.GetComponent<Collider2D>());
        groundLayerMask = LayerMask.GetMask("Ground");
        if (pattern)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            KillCountdown();
        }
        
    }

    private void Update()
    {
        actualPos = transform.position;
        distance = Vector2.SqrMagnitude(iniPosition - new Vector3(player.transform.position.x, player.transform.position.y));
        distPlayer = Vector2.SqrMagnitude(actualPos - new Vector3(player.transform.position.x, player.transform.position.y));
        Vector2 movement;

        if (!IsCoolingDown())
        {
            if (distance < rangeOfVision && distPlayer > rangeOfAttack)
            {
                NewObjective(player.transform.position.x, player.transform.position.y);
            }
            else if (distPlayer <= rangeOfAttack)
            {
                Debug.Log("atacando "+ distance + "");
                Cooldown();
                this.Attack(rangeAttack);
            }
            else 
            {
                if (Vector3.SqrMagnitude(transform.position - nextPosition) < 0.5)
                {
                    
                    if (pattern)
                    {
                        if (right)
                        {
                            NewObjective(transform.position.x + movX, transform.position.y + movY);
                        }
                        else
                        {
                            NewObjective(transform.position.x - movX, transform.position.y + movY);
                        }
                        movY = -movY;
                    }
                    else
                    {
                        Cooldown();
                        NewObjective();
                    }
                }
            }


            if (movementController.canFly)
            {
                movement = (nextPosition - transform.position).normalized;
            }
            else
            {
                movement = (nextPosition - transform.position);
                movement.y = 0f;
                movement = movement.normalized;
                RaycastHit2D hit;

                var raycastOrigin = new Vector3(0, collider.bounds.min.y);
                raycastOrigin.x = collider.bounds.center.x + movement.x * raycastFloorCheckDistance;

                hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 4f, groundLayerMask);

                bool v = hit.collider != null;
                if (!v)
                {   
                    Debug.Log("no toco suelo");
                    movement.x -= movement.x * 2;
                    Cooldown();
                }

            }

        } else
        {
            movement = new Vector3();

        }
        if (!pattern)
        {
            if (transform.position.x <= player.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        movementController.Move(movement.x, movement.y);
    }

    private void NewObjective()
    {
        nextPosition = iniPosition + new Vector3(Random.Range(-distGen, distGen), Random.Range(-distGen, distGen)); // TODO Generar valores random        
    }


    private void NewObjective(float objectiveX, float objectiveY)
    {
        nextPosition = new Vector3(objectiveX, objectiveY);
    }

    protected override void Die()
    {
        Destroy(this.gameObject);
        Debug.Log("me muero");
    }   

    public void Cooldown()
    {
        cooldown = true;
        cooldownIniTime = Time.realtimeSinceStartup;
    }

    private bool IsCoolingDown()
    {
        if (cooldown)
        {
            if (Time.realtimeSinceStartup - cooldownIniTime < attackCooldown)
            {
                return cooldown;
            }

            cooldown = false;
        }

        return cooldown;
    }
    
    async private void KillCountdown()
    {        
        await Task.Delay(timeToDie);
        Die();
    }

    private void Attack(bool rangeAttack)
    {
        if (rangeAttack)
        {
            rotation = Random.Range(rotMin, rotMax);
            //if (spawner.transform.position.x <= this.transform.position.x)
                //Instantiate(prefabBala, transform.TransformPoint(spawner.transform.position), Quaternion.identity, this.transform);

            var bala = Instantiate(prefabBala);
            // , spawner.transform.position, Quaternion.identity, transform.parent
            bala.transform.position = spawner.transform.position;
            bala.transform.rotation = Quaternion.identity;
            bala.transform.parent = transform.parent;   

            /*else
                Instantiate(prefabBala, spawner.transform.position, Quaternion.Euler(0, 0, -rotation));*/
        }
        else
        {
            Attack();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //player.Hurt(damage);
        }

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.Hurt(damage);
        }
    }


}
