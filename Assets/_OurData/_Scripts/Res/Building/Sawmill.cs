using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMill : WareHouse
{
    public override ResHolder ResNeedToMove()
    {
        ResHolder resHolder = this.GetResource(ResourceName.plank);
        if (resHolder.ResCurrent() > 0) return resHolder;
        return null;
    }
    public override List<Resource> NeedResource()
    {
        List<Resource> resources = new List<Resource>();


        ResHolder logwood = this.GetResource(ResourceName.logwood);
        Resource resLogwood = new Resource
        {
            name = logwood.Name(),
            number = logwood.resMax - logwood.ResCurrent()
        };

        if (resLogwood.number > 0) resources.Add(resLogwood);

        return resources;
    }
}
