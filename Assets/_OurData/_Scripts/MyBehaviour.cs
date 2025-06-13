using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void Start()
    {
        //For Override
    }

    protected virtual void Reset()
    {
        this.LoadComponents();
    }

    protected virtual void FixedUpdate()
    {
        //For Override
    }
    protected virtual void LoadComponents()
    {
        //For Override
    }
    protected virtual void OnDisable()
    {
        //For Override
    }
}
