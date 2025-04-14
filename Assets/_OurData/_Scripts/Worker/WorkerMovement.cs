using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerMovement : MyBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected NavMeshAgent navMeshAgent;



    protected override void FixedUpdate()
    {
        this.Moving();
    }
    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }

    protected virtual void Moving()
    {
        this.navMeshAgent.SetDestination(this.target.position);
    }
}
