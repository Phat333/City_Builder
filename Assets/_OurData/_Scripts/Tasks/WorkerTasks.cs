﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTasks : MyBehaviour
{
    public WorkerCtrl workerCtrl;
    public bool isNightTime = false;
    public bool inHouse = false;
    public bool readyForTask = false;
    public WorkerTask taskWorking;
    public WorkerTask taskGoHome;
    public Transform taskTarget;
    public BuildingCtrl TaskBuildingCtrl;
    [SerializeField] protected List<TaskType> tasks;

    protected override void Awake()
    {
        base.Awake();
        this.DisableTasks();
    }

    //protected virtual void Testting()
    //{
    //    this.isNightTime = !this.isNightTime;
    //}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (this.isNightTime) this.GoHome();
        else this.GotoWork();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerCtrl();
        this.LoadTasks();
    }

    protected virtual void LoadWorkerCtrl()
    {
        if (this.workerCtrl != null) return;
        this.workerCtrl = GetComponent<WorkerCtrl>();
        Debug.Log(transform.name + " : Load WorkerCtrl", gameObject);
    }

    protected virtual void LoadTasks()
    {
        if (this.taskWorking != null) return;
        Transform tasksObj = transform.Find("Tasks");
        this.taskWorking = tasksObj.GetComponentInChildren<TaskWorking>();
        this.taskGoHome = tasksObj.GetComponentInChildren<TaskGoHome>();

        Debug.Log(transform.name + " : Load WorkerTask", gameObject);
    }

    protected virtual void DisableTasks()
    {
        this.taskWorking.gameObject.SetActive(false);
        this.taskGoHome.gameObject.SetActive(false);
    }

    protected virtual void GoHome()
    {
        this.taskWorking.gameObject.SetActive(false);
        this.taskGoHome.gameObject.SetActive(true);
    }

    protected virtual void GotoWork()
    {
        this.taskWorking.gameObject.SetActive(true);
        this.taskGoHome.gameObject.SetActive(false);
    }

    public virtual void TaskAdd(TaskType taskType)
    {
        TaskType currentTask = this.TaskCurrent();
        if (taskType == currentTask) return;
        this.tasks.Add(taskType);
    }

    public virtual void TaskCurrentDone()
    {
        if (this.tasks.Count <= 0) return;
        this.tasks.RemoveAt(this.tasks.Count - 1);
    }

    public virtual TaskType TaskCurrent()
    {
        if (this.tasks.Count <= 0) return TaskType.None;
        return this.tasks[this.tasks.Count - 1];
    }

}
