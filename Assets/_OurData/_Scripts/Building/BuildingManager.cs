using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MyBehaviour
{
    public static BuildingManager Instance;
    [SerializeField] protected List<BuildingCtrl> buildingCtrls;

    protected override void Awake()
    {
        base.Awake();
        if(BuildingManager.Instance != null) Debug.LogError("Only 1 BuildingManager allow");
        BuildingManager.Instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBuildingCtrls();
    }

    protected virtual void LoadBuildingCtrls()
    {
        if (this.buildingCtrls.Count > 0) return;
        foreach(Transform child in this.transform)
        {
            BuildingCtrl ctrl = child.GetComponent<BuildingCtrl>();
            if (ctrl == null) continue;
            this.buildingCtrls.Add(ctrl);
        }
        Debug.Log(transform.name + " : Load BuildingCtrls", gameObject);
    }
    public virtual BuildingCtrl FindBuilding(WorkerCtrl worker, BuildingType buildingType)
    {
        BuildingCtrl buildingCtrl;
        for (int i = 0; i < this.buildingCtrls.Count; i++) 
        {
            buildingCtrl = this.buildingCtrls[i];
            if (!buildingCtrl.workers.IsNeedWorker()) continue; 
            //if (buildingCtrl.wareHouse.buildingType != buildingType) continue;

            buildingCtrl.workers.AddWorker(worker);

            return buildingCtrl;
            
        }
        return null;
    }
}
