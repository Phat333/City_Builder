using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : ResGenerator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadResCreate();
        this.SetLimit();
    }

    protected virtual void LoadResCreate()
    {
        Resource res = new Resource();
        {
            res.name = ResourceName.logwood;
            res.number = 1;
        }


        this.resCreate.Clear();
        this.resCreate.Add(res);
    }
    protected virtual void SetLimit()
    {
        ResHolder resHolder = this.GetHolder(ResourceName.logwood);
        resHolder.SetLimit(10);
    }
}
