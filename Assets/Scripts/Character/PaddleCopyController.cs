using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleCopyController : MonoBehaviour
{

    private Animator _animator;
    private SpriteRenderer _sr;
    [SerializeField]
    private PaddleController originalPaddleController;
    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(originalPaddleController != null && !initialized)
        {
            initialized = true;
            originalPaddleController.AddListener(this);
        }
    }

    public void Side(bool side)
    {
        _sr.flipX = side;
    }

    public void SetAction(Action<Animator> action)
    {
        action(_animator);
    }
}
