using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDrop : MonoBehaviour
{

    //public float gravity = 0;
    public float velocity = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //velocity += gravity * Time.time;
        transform.position -= new Vector3(0, velocity * Time.deltaTime, 0);
    }
}
