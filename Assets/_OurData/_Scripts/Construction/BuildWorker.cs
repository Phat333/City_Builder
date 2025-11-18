using UnityEngine;

public class BuildWorker : AbstractConstruction
{
    protected override void LoadBuildNames()
    {
        if (this.buildNames.Count > 0) return;
        this.buildNames.Add("WoodCutter");
        Debug.Log(transform.name + ": LoadBuildNames", gameObject);
    }

    protected override Transform FinishBuild()
    {
        Transform newBuild = base.FinishBuild();
        newBuild.parent = WorkerManager.instance.transform;
        return newBuild;
    }
}
