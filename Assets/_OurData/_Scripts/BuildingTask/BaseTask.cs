using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTask : BuildingTask
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.makingResource:
                Debug.Log("Making Resource");
                break;
            default:
                if (this.IsTimeToWork()) this.Planning(workerCtrl);
                break;
        }
    }

    protected virtual void Planning(WorkerCtrl workerCtrl)
    {
        Debug.Log("Planning for Base Task");
        // Implement planning logic here
        // For example, check if there are resources available, if not, go to gather resources
        // If resources are available, start making the resource
    }
}
