using System.Collections.Generic;
using UnityEngine;

public class HouseBuilderTask : BuildingTask
{
    [Header("House Builder")]
    [SerializeField] protected AbstractConstruction construction;
    [SerializeField] protected List<BuildingCtrl> warehouses;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
 
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.findWarehouseHasRes:
                this.FindWarehouseHasRes(workerCtrl);
                break;
            case TaskType.getResNeedToMove:
                this.GetResNeedToMove(workerCtrl);
                break;
            case TaskType.bringResourceBack:
                this.BringResToConstruction(workerCtrl);
                break;
            case TaskType.buildConstruction:
                this.BuildConstruction(workerCtrl);
                break;
            case TaskType.goToWorkStation:
                this.BackToWorkStation(workerCtrl);
                break;
            default:
                if (this.IsTimeToWork()) this.Planning(workerCtrl);
                break;
        }
    }

    protected virtual void Planning(WorkerCtrl workerCtrl)
    {
        if (this.construction == null) this.construction = ConstructionManager.instance.GetConstruction();
        if (this.construction)
        {
            this.construction.builder = this.buildingCtrl;
            workerCtrl.workerTasks.TaskAdd(TaskType.findWarehouseHasRes);
            this.FindWarehouse();

        }
    }

    protected virtual void FindWarehouse()
    {
        List<BuildingCtrl> buildingCtrls = BuildingManager.Instance.BuildingCtrls();
        foreach(BuildingCtrl buildingCtrl in buildingCtrls)
        {
            if (buildingCtrl.buildingTask.GetType() != typeof(WareHouseTask)) continue;
            if (this.warehouses.Contains(buildingCtrl)) continue;
            this.warehouses.Add(buildingCtrl);
        }
    }

    protected virtual void FindWarehouseHasRes(WorkerCtrl workerCtrl)
    {
        ResourceName resRequireName = this.construction.GetResRequireName();

        foreach (BuildingCtrl warehouse in this.warehouses)
        {
            ResHolder resHolder = warehouse.wareHouse.GetResource(resRequireName);
            if (resHolder.ResCurrent() < 1) continue;
            workerCtrl.workerTasks.taskBuildingCtrl = warehouse;
            workerCtrl.workerTasks.TaskCurrentDone();
            workerCtrl.workerTasks.TaskAdd(TaskType.getResNeedToMove);
            return;
        }
    }

    protected virtual void GetResNeedToMove(WorkerCtrl workerCtrl)
    {
        BuildingCtrl warehouseCtrl = workerCtrl.workerTasks.taskBuildingCtrl;

        ResourceName resRequireName = this.construction.GetResRequireName();

        
        ResHolder resHolder = warehouseCtrl.wareHouse.GetResource(resRequireName);
        if (resRequireName == ResourceName.noResource)
        {
            workerCtrl.workerMovement.SetTarget(null);
            //workerCtrl.workerTasks.TaskCurrentDone();
            //workerCtrl.workerTasks.TaskAdd(TaskType.buildConstruction);
            return;
        }
        if (resHolder.ResCurrent() < 1)
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            //workerCtrl.workerTasks.TaskAdd(TaskType.findWarehouseHasRes);
            return;
        }

        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null) workerCtrl.workerMovement.SetTarget(warehouseCtrl.door);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        //workerCtrl.workerTasks.TaskCurrentDone();
        int carryCount = workerCtrl.resCarrier.carryCount;
        warehouseCtrl.wareHouse.RemoveResource(resRequireName, carryCount);
        workerCtrl.resCarrier.AddResource(resRequireName, carryCount);

        workerCtrl.workerTasks.TaskCurrentDone();
        workerCtrl.workerMovement.SetTarget(null);
        workerCtrl.workerTasks.TaskAdd(TaskType.bringResourceBack);
        
    }

    protected virtual void BringResToConstruction(WorkerCtrl workerCtrl)
    {
        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null) workerCtrl.workerMovement.SetTarget(this.construction.transform);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        //workerCtrl.workerMovement.SetTarget(null);
        workerCtrl.workerTasks.TaskCurrentDone();
        Resource res = workerCtrl.resCarrier.TakeFirst();
        this.construction.AddRes(res.name, res.number);

        ResourceName resRequireName = this.construction.GetResRequireName();
        if (resRequireName == ResourceName.noResource)
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.buildConstruction);
            return;

        }
        workerCtrl.workerTasks.TaskAdd(TaskType.findWarehouseHasRes);

    }

    protected virtual void BuildConstruction(WorkerCtrl workerCtrl)
    {
        if (!this.IsContrucstionFinish()) return;

        workerCtrl.workerTasks.TaskCurrentDone();
        workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
        //Debug.Log("BuildConstruction");
        //float percent = this.construction.Percent();
        //if (percent < 99) return;
        //{
        //    workerCtrl.workerTasks.TaskCurrentDone();
        //    workerCtrl.workerMovement.SetTarget(null);
        //    workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);

        //    this.construction.Finish();
        //    this.construction = null;
        //}
        //Debug.Log("Percent: " + percent);
    }

    protected virtual bool IsContrucstionFinish()
    {
        if (this.construction == null) return true;

        float percent = this.construction.Percent();
        if (percent < 99) return false;

        this.construction.Finish();
        this.construction = null;
        return true;
    }


}
