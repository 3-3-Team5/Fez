using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Animator _animator;
    [SerializeField] private float disappearingTime;
    [SerializeField] private float resettingTime;
    private Color originColor;
    private Color endColor;
    [HideInInspector]public bool isTouch = false;
    
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        originColor = _meshRenderer.material.color;
        _animator.speed = 1 / disappearingTime;
    }

    public void StartAnim()
    {
        _animator.SetBool("IsTouch",true);
    }

    public void InvokeReset()
    {
        _collider.enabled = false;
        _meshRenderer.enabled = false;
        Invoke(nameof(Resetting),resettingTime);   
    }

    private void Resetting()
    {
        _collider.enabled = true;
        _meshRenderer.enabled = true;
        _meshRenderer.material.color = originColor;
        _animator.SetBool("IsTouch",false);
    }

   
}
