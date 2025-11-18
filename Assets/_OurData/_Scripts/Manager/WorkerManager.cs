using UnityEngine;

public class WorkerManager : MyBehaviour
{
    public static WorkerManager instance;
    protected override void Awake()
    {
        base.Awake();
        if (WorkerManager.instance != null) Debug.LogError("Only 1 WorkerManager allow");
        WorkerManager.instance = this;
    }
}
