using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraModifier : MonoBehaviour
{
    [SerializeField]
    public float depth;
    float _depth;

    [SerializeField]
    Vector2 offset = Vector2.zero;
    Vector2 _offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _offset = CustomCamera.instance.offset;
            CustomCamera.instance.offset = offset;

            _depth = CustomCamera.instance.depth;
            CustomCamera.instance.depth = depth;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            CustomCamera.instance.offset = _offset;
            CustomCamera.instance.depth = _depth;
        }
    }
}
