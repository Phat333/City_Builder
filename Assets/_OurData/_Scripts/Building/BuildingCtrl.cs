using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCtrl : MyBehaviour
{
    [Header("Building")]
    public BuildingType buildingType = BuildingType.workStation;
    public Transform door;
    public Workers workers;
    public WareHouse wareHouse;
    public BuildingTask buildingTask;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkers();
        this.LoadDoor();
        this.LoadWareHouse();
        this.LoadBuildingTask();
    }

    protected virtual void LoadWorkers()
    {
        if (this.workers != null) return;
        this.workers = GetComponent<Workers>();
        Debug.Log(transform.name + " : Load Workers", gameObject);
    }
    protected virtual void LoadDoor()
    {
        if (this.door != null) return;
        this.door = transform.Find("Door");
        Debug.Log(transform.name + " : Load Door", gameObject);
    }

    protected virtual void LoadWareHouse()
    {
        if (this.wareHouse != null) return;
        this.wareHouse = GetComponent<WareHouse>();
        Debug.Log(transform.name + " : Load WareHouse", gameObject);
    }

    protected virtual void LoadBuildingTask()
    {
        if (this.buildingTask != null) return;
        this.buildingTask = GetComponent<BuildingTask>();
        Debug.Log(transform.name + " : Load Building Task", gameObject);
    }


}
