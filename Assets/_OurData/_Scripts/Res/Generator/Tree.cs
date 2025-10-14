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
        //this.buildingType = BuildingType.resource;
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
        ResHolder resHolder = this.GetResource(ResourceName.logwood);
        resHolder.SetLimit(10);
    }
    
}
