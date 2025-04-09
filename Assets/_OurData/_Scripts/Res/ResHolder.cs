using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResHolder : MyBehaviour
{
    [SerializeField] protected ResourceName ResourceName;
    [SerializeField] protected float resCurrent = 0;
    [SerializeField] protected float resMax = Mathf.Infinity;

    protected override void LoadComponents()
    {
        this.LoadResName();
    }
    protected virtual void LoadResName()
    {
        string name = transform.name;
        this.ResourceName = ResNameParser.FromString(name);
    }

}
