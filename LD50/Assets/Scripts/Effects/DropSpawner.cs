using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    public GameObject lavaDrop;
    public float period = 3f;

    float timeSinceLast;

    private bool visible = true;

    Animator animator;
    List<GameObject> lavaDrops;
    int iDrop = 0;

    // Start is called before the first frame update
    void Start()
    {
        //timeSinceLast = Time.realtimeSinceStartup + timeOffset;
        animator = GetComponent<Animator>();

        lavaDrops = new List<GameObject>();

        lavaDrops.Add(Instantiate(lavaDrop));
        lavaDrops.Add(Instantiate(lavaDrop));
        lavaDrops.Add(Instantiate(lavaDrop));
        lavaDrops.Add(Instantiate(lavaDrop));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - timeSinceLast >= period)
        {
            timeSinceLast = Time.realtimeSinceStartup;
            if (visible) animator.SetTrigger("spawn");
        }

    }


    //private void OnBecameVisible()
    //{
    //    visible = true;
    //}

    //private void OnBecameInvisible()
    //{
    //    visible = false;
    //}


    public void SpawnLavaDrop()
    {
        var drop = lavaDrops[iDrop];
        if (drop != null) drop.transform.position = gameObject.transform.position - new Vector3(0, 0.4f, 0);

        iDrop++;
        if (iDrop >= lavaDrops.Count)
            iDrop = 0;
    }
}
