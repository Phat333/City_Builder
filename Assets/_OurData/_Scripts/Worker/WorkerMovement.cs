using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerMovement : MyBehaviour
{
    public WorkerCtrl workerCtrl;
    [SerializeField] protected Transform target;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected Animator animator;
    public bool IsWalking = false;
    public bool IsWorking = false;

    [SerializeField] protected float walkLimit = 0.7f;
    [SerializeField] protected float targetDistance = 0f;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerCtrl();
        this.LoadAgents();
        this.LoadAnimator();

    }


    protected override void FixedUpdate()
    {
        this.Moving();
        this.Animating();

    }

    protected virtual void LoadWorkerCtrl()
    {
        if (this.workerCtrl != null) return;
        this.workerCtrl = GetComponent<WorkerCtrl>();
        Debug.Log(transform.name + " : Load WorkerCtrl", gameObject);
    }

    public virtual Transform GetTarget()
    {
        return this.target;
    }

    protected virtual void LoadAgents()
    {
        if (this.navMeshAgent != null) return;
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        Debug.Log(transform.name + " : Load NavMeshAgent", gameObject);
    }

    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = this.GetComponentInChildren<Animator>();
        Debug.Log(transform.name + " : Load Animator", gameObject);
    }
    public virtual void SetTarget(Transform trans)
    {
        this.target = trans;
    }

    protected virtual void Moving()
    {
        if (this.target == null || this.IsCloseToTarget())
        {
            this.navMeshAgent.isStopped = true;
            this.IsWalking = false;
            return;
        }

        this.IsWalking = true;
        this.navMeshAgent.isStopped = false;
        this.navMeshAgent.SetDestination(this.target.position);
    }
    public virtual bool IsCloseToTarget()
    {
        if (this.target == null) return false;

        Vector3 targetPos = this.target.position;
        targetPos.y = transform.position.y;

        this.targetDistance = Vector3.Distance(transform.position, targetPos);
        return this.targetDistance < this.walkLimit;
    }
    protected virtual void Animating()
    {
        this.workerCtrl.animator.SetBool("IsWalking", this.IsWalking);
        this.workerCtrl.animator.SetBool("IsWorking", this.IsWorking);
    }

    public virtual float TargetDistance()
    {
        return this.targetDistance;

    }
}

