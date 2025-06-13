using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestHut : WareHouse
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.buildingType = BuildingType.workStation;
    }
}
