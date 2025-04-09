using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : ResGenerator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadResCreate();
    }

    protected virtual void LoadResCreate()
    {
        Resource res = new Resource();
        {
            res.name = ResourceName.water;
            res.number = 1;
        }
        

        this.resCreate.Clear();
        this.resCreate.Add(res);
    }
}
