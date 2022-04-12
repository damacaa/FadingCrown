using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartComponent : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetActivo(true);
    }

    public void SetActivo(bool activo)
    {
        animator.SetBool("activo", activo);
    }
}
