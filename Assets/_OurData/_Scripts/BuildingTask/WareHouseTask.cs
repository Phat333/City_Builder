using System.Collections;
using UnityEngine;

public class WareHouseTask : BuildingTask
{
    //[Header("WareHouseTask")]

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.getResNeedToMove:
                this.GoGetResNeedToMove(workerCtrl);
                break;
            case TaskType.bringResourceBack:
                this.BringResourceBack(workerCtrl);
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
        BuildingCtrl buildingCtrl = this.GetWorkStationHasResNeedToMove();
        if (buildingCtrl != null)
        {
            workerCtrl.workerTasks.taskBuildingCtrl = buildingCtrl;
            workerCtrl.workerMovement.SetTarget(null);
            workerCtrl.workerTasks.TaskAdd(TaskType.getResNeedToMove);

        }
    }

    protected virtual void GoGetResNeedToMove(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();
        BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;
        ResHolder resHolder = taskBuildingCtrl.wareHouse.ResNeedToMove();
        if(resHolder == null)
        {
            this.DoneGetResNeedToMove(workerCtrl);
            return;
        }

        if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);

        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        float count = 1;

        resHolder.Deduct(count);
        workerCtrl.resCarrier.AddResource(resHolder.Name(), count);
        this.DoneGetResNeedToMove(workerCtrl);

        Resource res = workerCtrl.resCarrier.Resource()[0];
        BuildingCtrl buildingCtrl = this.FindBuildingNeedRes(resHolder.Name());
        workerTasks.taskBuildingCtrl = buildingCtrl;
        workerTasks.TaskAdd(TaskType.bringResourceBack);


    }

    protected virtual void DoneGetResNeedToMove(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerTasks.TaskCurrentDone();
        workerCtrl.workerTasks.taskBuildingCtrl = null;
        workerCtrl.workerMovement.SetTarget(null);
    }

    protected virtual BuildingCtrl GetWorkStationHasResNeedToMove()
    {
        foreach (BuildingCtrl buildingCtrl in BuildingManager.Instance.BuildingCtrls())
        {
            if (buildingCtrl.wareHouse.buildingType != BuildingType.workStation) continue;
            ResHolder resHolder = buildingCtrl.wareHouse.ResNeedToMove();

            if (resHolder == null) continue;
            return buildingCtrl;
        }
        return null;
    }

    protected virtual BuildingCtrl FindBuildingNeedRes(ResourceName resName)
    {
        foreach(BuildingCtrl buildingCtrl in BuildingManager.Instance.BuildingCtrls())
        {
            if (buildingCtrl.wareHouse.buildingType != BuildingType.workStation) continue;
            ResHolder resHolder = buildingCtrl.wareHouse.IsNeedRes(resName);
            if (resHolder == null) continue;
            return buildingCtrl;
        }
        return this.buildingCtrl;
    }

    protected virtual void BringResourceBack(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();
        BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;
        if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        workerCtrl.workerMovement.SetTarget(null);
        workerTasks.taskBuildingCtrl = null;
        workerTasks.TaskCurrentDone();

        Resource res = workerCtrl.resCarrier.TakeFirst();
        taskBuildingCtrl.wareHouse.AddResource(res.name, res.number);

        ResHolder resHolder = taskBuildingCtrl.wareHouse.ResNeedToMove();
        if (resHolder == null) return;
        workerTasks.taskBuildingCtrl = taskBuildingCtrl;
        workerTasks.TaskAdd(TaskType.getResNeedToMove);
    }
}
