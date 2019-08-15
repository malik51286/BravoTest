using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion: MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _animator.SetTrigger("Explode");
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
