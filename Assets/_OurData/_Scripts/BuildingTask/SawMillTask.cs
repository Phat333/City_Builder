using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SawMillTask : BuildingTask
{
    [Header("SawMillTask")]
    [SerializeField] protected Transform workingPoint;
    [SerializeField] protected float logWoodCost = 1;
    [SerializeField] protected float plankReceive = 2;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkingPoint();
    }

    protected virtual void LoadWorkingPoint()
    {
        if (this.workingPoint != null) return;
        this.workingPoint = transform.Find("WorkingPoint");
        Debug.Log(transform.name + " LoadWorkingPoint", gameObject);
    }


    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.makingResource:
                this.MakeingResource(workerCtrl);
                Debug.Log("SawMillTask makingResource");
                break;
            case TaskType.gotoWorkingPoint:
                this.GotoWorkingPoint(workerCtrl);
                Debug.Log("SawMillTask gotoWorkingPoint");
                break;
            case TaskType.goToWorkStation:
                this.BackToWorkStation(workerCtrl);
                break;
            default:
                if (this.IsTimeToWork()) this.Planning(workerCtrl);
                break;
        }
    }

    protected virtual void MakeingResource(WorkerCtrl workerCtrl)
    {

        if (workerCtrl.workerMovement.IsWorking) return;
        StartCoroutine(Sawing(workerCtrl));
        
    }

    IEnumerator Sawing(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerMovement.IsWorking = true;
        workerCtrl.workerMovement.workingType = WorkingType.sawWood;
        yield return new WaitForSeconds(this.workTimer);

        this.buildingCtrl.wareHouse.RemoveResource(ResourceName.logwood, this.logWoodCost);

        this.buildingCtrl.wareHouse.AddResource(ResourceName.plank, this.plankReceive);

        workerCtrl.workerMovement.IsWorking = false;
        workerCtrl.workerTasks.TaskCurrentDone();

    }

    protected virtual void Planning(WorkerCtrl workerCtrl)

    {
        if (!this.IsStoreFull()&& this.HasLogwood())
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
            workerCtrl.workerTasks.TaskAdd(TaskType.makingResource);
            workerCtrl.workerTasks.TaskAdd(TaskType.gotoWorkingPoint);
        }
    }



    protected virtual bool IsStoreFull()
    {
        return false;
    }
    protected virtual bool HasLogwood()
    {
        return true;
    }

    protected virtual void GotoWorkingPoint(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if(workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null) workerCtrl.workerMovement.SetTarget(this.workingPoint);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;
        workerCtrl.workerMovement.SetTarget(null);
        workerCtrl.workerTasks.TaskCurrentDone();
    }
}

