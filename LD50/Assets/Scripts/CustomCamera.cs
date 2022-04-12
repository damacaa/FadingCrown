using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public static CustomCamera instance;

    [SerializeField]
    public float depth;

    [SerializeField]
    GameObject target;
    [SerializeField]
    public Vector2 offset = Vector2.zero;

    [SerializeField]
    public Vector2 targetMovementMultiplier;
    [SerializeField]
    public float smoothTime;
    Vector2 camVelocity;
    [SerializeField]
    float a, b, k;

    Vector2 lastPos;

    private void Awake()
    {
        instance = this;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0)
            return;

        Vector2 globalPos = transform.position;
        Vector2 velocity = (globalPos - lastPos) / Time.deltaTime;
        lastPos = globalPos;

        Vector2 targetPos = target.transform.localPosition;

        float s = k / (1 + Mathf.Pow(2.71f, a - (b * velocity.y)));
        targetPos.y *= s;

        Vector2 dir = Vector2.SmoothDamp(transform.localPosition, offset + targetPos * targetMovementMultiplier, ref camVelocity, smoothTime);

        if(float.IsNaN(dir.y))
        {
            dir.y = 0;
        }

        transform.localPosition = new Vector3(dir.x, dir.y, depth);
    }


}
