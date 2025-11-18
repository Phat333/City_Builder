using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : WareHouse
{
    public override ResHolder ResNeedToMove()
    {
        ResHolder resHolder = this.GetResource(ResourceName.logwood);
        if (resHolder.ResCurrent() > 0) return resHolder;
        return null;
    }

    public override ResHolder IsNeedRes(Resource resource)
    {
        if (resource.name != ResourceName.logwood) return null;

        ResHolder resHolder = this.GetResource(resource.name);
        if (resHolder.IsMax()) return null;
        return resHolder;
    }
}
