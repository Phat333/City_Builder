using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTask : MyBehaviour
{
    public WorkerCtrl workerCtrl;
    [SerializeField] protected float buildingDistance = 0;
    [SerializeField] protected float buildingDisLimit = 0.7f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (this.GetBuilding()) this.GettingReadyForWork();
        else this.FindBuilding();

        if(workerCtrl.workerTasks.readyForTask) this.Working();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //this.GoOutBuilding();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerCtrl();
    }

    protected virtual void LoadWorkerCtrl()
    {
        if (this.workerCtrl != null) return;
        this.workerCtrl = GetComponentInParent<WorkerCtrl>();
        Debug.Log(transform.name + ": LoadWorkerCtrl", gameObject);
    }

    protected virtual void FindBuilding()
    {
        BuildingCtrl buildingCtrl = BuildingManager.Instance.FindBuilding(workerCtrl, this.GetBuildingType());
        if (buildingCtrl == null) return;

        this.AssignBuilding(buildingCtrl);
    }

    public virtual void GotoBuilding()
    {
        BuildingCtrl buildingCtrl = this.GetBuilding();
        this.workerCtrl.workerMovement.SetTarget(buildingCtrl.door);
    }

    public virtual bool IsAtBuilding()
    {
        return this.BuildingDistance() < this.buildingDisLimit;
    }

    protected virtual float BuildingDistance()
    {
        BuildingCtrl buildingCtrl = this.GetBuilding();
        this.buildingDistance = Vector3.Distance(this.transform.position, buildingCtrl.door.transform.position);
        return this.buildingDistance;
    }

    protected virtual void GettingReadyForWork()
    {
        if(this.workerCtrl.workerTasks.readyForTask) return;

        if (!this.workerCtrl.workerMovement.IsCloseToTarget())
        {
            this.GotoBuilding();
            return;
        }
        //this.workerCtrl.workerMovement.SetTarget(null);
        this.workerCtrl.workerTasks.readyForTask = true;
        this.GoIntoBuilding();

    }

    //protected virtual void WorkPlanning()
    //{
    //    if (this.IsAtBuilding()) this.GoIntoBuilding();
    //    else this.GotoBuilding();
    //    if (this.inHouse) this.Working();
    //}

    public virtual void GoIntoBuilding()
    {
        if (this.workerCtrl.workerTasks.inHouse) return;

        this.workerCtrl.workerMovement.SetTarget(null);
        this.workerCtrl.workerTasks.inHouse = true;
        this.workerCtrl.workerModel.gameObject.SetActive(false);
    }

    public virtual void GoOutBuilding()
    {
        if (!this.workerCtrl.workerTasks.inHouse) return;
        this.workerCtrl.workerTasks.inHouse = false;
        this.workerCtrl.workerModel.gameObject.SetActive(true);
    }
    protected virtual void Working()
    {
        this.GetBuilding().buildingTask.DoingTask(this.workerCtrl);
    }

    protected virtual BuildingCtrl GetBuilding()
    {
        return null;
    }

    protected virtual void AssignBuilding(BuildingCtrl buildingCtrl)
    {
    }

    protected virtual BuildingType GetBuildingType()
    {
        return BuildingType.workStation;
    }
}
