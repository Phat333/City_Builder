using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestHut : WareHouse
{
    public override ResHolder ResNeedToMove()
    {
        ResHolder resHolder = this.GetResource(ResourceName.logwood);
        if (resHolder.ResCurrent() > 0) return resHolder;
        return null;
    }

    
}
