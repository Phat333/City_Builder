using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTask : MyBehaviour
{
    [Header("Building Task")]
    public BuildingCtrl buildingCtrl;
    [SerializeField] protected float taskTimer = 0;
    [SerializeField] protected float taskDelay = 5f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBuildingCtrl();
    }

    protected virtual void LoadBuildingCtrl()
    {
        if (this.buildingCtrl != null) return;
        this.buildingCtrl = GetComponentInParent<BuildingCtrl>();
        Debug.Log(transform.name + " : Load Building Ctrl", gameObject);
    }

    protected virtual bool IsTimeToWork()
    {
        this.taskTimer += Time.fixedDeltaTime;
        if (this.taskTimer < this.taskDelay) return false;
        this.taskTimer = 0;
        return true;
    }

    protected virtual void BackToWorkStation(WorkerCtrl workerCtrl)
    {
        WorkerTask taskWorking = workerCtrl.workerTasks.taskWorking;
        taskWorking.GotoBuilding();
        if (workerCtrl.workerMovement.IsCloseToTarget())
        {
            taskWorking.GoIntoBuilding();
            workerCtrl.workerTasks.TaskCurrentDone();
        }
    }

    public virtual void DoingTask(WorkerCtrl workerCtrl)
    {
        //For Override
    }
}
