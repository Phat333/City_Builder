using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMill : WareHouse
{
    public override ResHolder ResNeedToMove()
    {
        ResHolder resHolder = this.GetResource(ResourceName.logwood);
        if (resHolder.ResCurrent() > 0) return resHolder;
        return null;
    }
    public override ResHolder IsNeedRes(ResourceName resName)
    {
        if (resName != ResourceName.logwood) return null;
        ResHolder resHolder = this.GetResource(resName);
        if (resHolder.IsMax()) return null;
        return resHolder;
    }
}
