using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemigoSpawneable;
    public float timeSpawn;
    float timePast = 0f;
    public float visionX = 40f;
    public float visionY = 150f;
    GameObject player;
    public bool right;

    // Start is called before the first frame update
    void Start()
    {
        //spawnPoint = GetComponentInChildren<Transform>().position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timePast += Time.deltaTime;

        float x = Mathf.Abs(player.transform.position.x - transform.position.x);
        float y = Mathf.Abs(player.transform.position.y - transform.position.y);

        if (x < visionX && y < visionY)
        {

            if (timePast >= timeSpawn)
            {
                var enemigo = Instantiate(enemigoSpawneable) as GameObject;
                enemigo.transform.position = transform.position;
                enemigo.transform.rotation = Quaternion.identity;
                enemigo.transform.parent = transform.parent;
                enemigo.GetComponent<Enemy>().direction = this.right ? 1 : -1;

                timePast = 0f;
            }
        }
        
    }
}
