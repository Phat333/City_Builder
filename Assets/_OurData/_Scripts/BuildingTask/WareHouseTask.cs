using System.Collections.Generic;
using UnityEngine;

public class WareHouseTask : BuildingTask
{
    [Header("WareHouseTask")]
    [SerializeField] protected int takeProductCount = 0;
    [SerializeField] protected int takeProductMax = 7;
    [SerializeField] protected float takeProducTimer = 0;
    [SerializeField] protected float takeProductDelay = 7f;

    [SerializeField] protected int bringMaterialCount = 0;
    [SerializeField] protected int bringMaterialMax = 2;
    [SerializeField] protected float bringMaterialTimer = 0;
    [SerializeField] protected float bringMaterialDelay = 7f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        //switch (workerCtrl.workerTasks.TaskCurrent())
        //{
        //    case TaskType.getResNeedToMove:
        //        this.GoGetResNeedToMove(workerCtrl);
        //        break;
        //    case TaskType.bringResourceBack:
        //        this.BringResourceBack(workerCtrl);
        //        break;
        //    case TaskType.goToWorkStation:
        //        this.BackToWorkStation(workerCtrl);
        //        break;
        //    default:
        //        if (this.IsTimeToWork()) this.Planning(workerCtrl);
        //        break;
        //}

        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.findBuildingHasProduct:
                this.FindBuildingHasProduct(workerCtrl);
                break;
            case TaskType.gotoGetProduct:
                this.GotoGetProduct(workerCtrl);
                break;
            case TaskType.takingProductBack:
                this.BringResourceBack(workerCtrl);
                break;
            case TaskType.findBuildingNeedMaterial:
                this.FindBuildingNeedMaterial(workerCtrl);
                break;
            case TaskType.bringMaterialToBuilding:
                this.BringMaterialToBuilding(workerCtrl);
                break;
            case TaskType.goToWorkStation:
                this.GotoWorkStation(workerCtrl);
                break;
            default:
                if (this.IsTimeToWork()) this.Planning(workerCtrl);
                break;
        }

    }

    protected virtual void Planning(WorkerCtrl workerCtrl)
    {
        //BuildingCtrl buildingCtrl = this.GetWorkStationHasResNeedToMove();
        //BuildingCtrl buildingCtrl = this.GetNextBuildingToWork();
        //if (buildingCtrl != null)
        //{
        //    workerCtrl.workerTasks.taskBuildingCtrl = buildingCtrl;
        //    workerCtrl.workerMovement.SetTarget(null);
        //    workerCtrl.workerTasks.TaskAdd(TaskType.getResNeedToMove);

        //}
        workerCtrl.workerTasks.TaskAdd(TaskType.findBuildingNeedMaterial);
        workerCtrl.workerTasks.TaskAdd(TaskType.findBuildingHasProduct);

        this.bringMaterialCount = this.bringMaterialMax;
        this.takeProductCount = this.takeProductMax;

    }

    protected virtual void FindBuildingHasProduct(WorkerCtrl workerCtrl)
    {
        this.takeProducTimer += Time.fixedDeltaTime;
        if (this.takeProducTimer > this.takeProductDelay)
        {
            this.takeProductCount--;
            this.takeProducTimer = 0;

        }
        if (this.takeProductCount < 0)
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            return;
        }

        BuildingCtrl buildingCtrl = this.FindBuildingHasProductOld(workerCtrl);
        if (buildingCtrl != null)
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.gotoGetProduct);
            this.takeProducTimer = 0;
            this.takeProductCount--;
        }
    }

    protected virtual void FindBuildingNeedMaterial(WorkerCtrl workerCtrl)
    {
        this.bringMaterialTimer += Time.fixedDeltaTime;
        if (this.bringMaterialTimer > this.bringMaterialDelay)
        {
            this.bringMaterialCount--;
            this.bringMaterialTimer = 0;
        }

        if (this.bringMaterialCount < 0)
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            return;
        }

        List<Resource> resources;
        ResHolder resHolder;
        int carryCount = workerCtrl.resCarrier.carryCount;

        foreach(BuildingCtrl buildingCtrl in this.nearBuildings)
        {
            if (buildingCtrl.buildingType != BuildingType.workStation) continue;
            resources = buildingCtrl.wareHouse.NeedResource();
            foreach(Resource resource in resources)
            {
                resHolder = this.buildingCtrl.wareHouse.GetResource(resource.name);
                if (resHolder.ResCurrent() < 1) continue;

                this.buildingCtrl.wareHouse.RemoveResource(resource.name, carryCount);
                workerCtrl.workerTasks.taskBuildingCtrl = buildingCtrl;
                workerCtrl.workerTasks.TaskAdd(TaskType.bringMaterialToBuilding);

                this.bringMaterialCount--;
                this.bringMaterialTimer = 0;
                return;
            }
        }
    }

    protected virtual void GotoGetProduct(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

        BuildingCtrl taskbuildingCtrl = workerTasks.taskBuildingCtrl;
        ResHolder resHolder = taskbuildingCtrl.wareHouse.ResNeedToMove();
        if (resHolder == null)
        {
            this.DoneGetResNeedToMove(workerCtrl);
            return;
        }

        if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskbuildingCtrl.door);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        float count = workerCtrl.resCarrier.carryCount;
        resHolder.Deduct(count);
        workerCtrl.resCarrier.AddResource(resHolder.Name(), count);
        this.DoneGetResNeedToMove(workerCtrl);

        workerTasks.taskBuildingCtrl = this.buildingCtrl;
        workerTasks.TaskAdd(TaskType.takingProductBack);
    }


    protected virtual void DoneGetResNeedToMove(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerTasks.TaskCurrentDone();
        workerCtrl.workerTasks.taskBuildingCtrl = null;
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

    protected virtual BuildingCtrl FindBuildingHasProductOld(WorkerCtrl workerCtrl)
    {
        int tryCount = 999;
        do
        {
            tryCount--;
            this.lastBuildingWorked++;
            if (lastBuildingWorked >= this.nearBuildings.Count)
            {
                this.lastBuildingWorked = 0;
                break;
            }

            BuildingCtrl nextBuilding = this.nearBuildings[this.lastBuildingWorked];
            if (nextBuilding.buildingType != BuildingType.workStation) continue;

            ResHolder resHolder = nextBuilding.wareHouse.ResNeedToMove();
            if (resHolder == null) continue;
            workerCtrl.workerTasks.taskBuildingCtrl = nextBuilding;
            return nextBuilding;
            
        }while (tryCount > 0);

        return null;
    }


    

    protected virtual void BringResourceBack(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();
        BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;
        if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        workerTasks.taskBuildingCtrl = null;
        workerTasks.TaskCurrentDone();

        Resource res = workerCtrl.resCarrier.TakeFirst();
        taskBuildingCtrl.wareHouse.AddResource(res.name, res.number);

        workerTasks.TaskAdd(TaskType.goToWorkStation);
    }

    protected virtual void BringMaterialToBuilding(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

        BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;
        if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;
        Resource res = workerCtrl.resCarrier.TakeFirst();
        taskBuildingCtrl.wareHouse.AddResource(res.name, res.number);

        workerTasks.taskBuildingCtrl = null;
        workerTasks.TaskCurrentDone();

        workerTasks.TaskAdd(TaskType.goToWorkStation);
    }
}
