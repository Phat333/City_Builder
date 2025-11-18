using UnityEngine;

public class Buildbuilding : AbstractConstruction
{
    protected override Transform FinishBuild()
    {
        Transform newBuild = base.FinishBuild();
        BuildingCtrl buildingCtrl = newBuild.GetComponent<BuildingCtrl>();
        BuildingManager.Instance.AddBuilding(buildingCtrl);
        return newBuild;
    }

    protected override void Building()
    {
        if (this.percent < 99) base.Building();
    }
}
