using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ForestHutTask : BuildingTask
{
    [Header("Forest Hut")]
    [SerializeField] protected GameObject plantTreeObj;
    [SerializeField] protected float treeRange = 27f;
    [SerializeField] protected float treeDistance = 7f;
    [SerializeField] protected List<GameObject> trees;
    [SerializeField] protected int treeMax = 7;
    //[SerializeField] protected float treeRemoveSpeed = 16;
    [SerializeField] protected List<GameObject> treePrefabs;
    [SerializeField] protected int storeMax = 7;
    [SerializeField] protected int storeCurrent = 0;
    [SerializeField] protected float chopSpeed = 7;

    protected override void Start()
    {
        base.Start();
        this.LoadNearByTree();
    }



    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObjects();
        this.LoadTreePrefabs();
    }

    protected virtual void LoadObjects()
    {
        if (this.plantTreeObj != null) return;
        this.plantTreeObj = Resources.Load<GameObject>("Building/MaskPositionObject");
        Debug.Log(transform.name + " : Load Plant Tree Object", gameObject);
    }

    protected virtual void LoadTreePrefabs()
    {
        if (this.treePrefabs.Count > 0) return;
        GameObject tree1 = Resources.Load<GameObject>("Res/Tree_1");
        GameObject tree2 = Resources.Load<GameObject>("Res/Tree_2");
        GameObject tree3 = Resources.Load<GameObject>("Res/Tree_3");
        this.treePrefabs.Add(tree1);
        this.treePrefabs.Add(tree2);
        this.treePrefabs.Add(tree3);
        Debug.Log(transform.name + " LoadObjects", gameObject);
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.plantTree:
                this.PlantTree(workerCtrl);
                break;
            case TaskType.findTreeToChop:
                this.FindTreeToChop(workerCtrl);
                break;
            case TaskType.chopTree:
                this.ChopTree(workerCtrl);
                break;
            case TaskType.goToWorkStation:
                this.BackToWorkStation(workerCtrl);
                break;
            default:
                if (this.IsTimeToWork()) this.Planning(workerCtrl);
                break;
        }
    }

    protected virtual void Planning(WorkerCtrl workerCtrl)

    {
        if(this.NeedMoreTree()) workerCtrl.workerTasks.TaskAdd(TaskType.plantTree);
        if (!this.IsStoreFull())
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.chopTree);
            workerCtrl.workerTasks.TaskAdd(TaskType.findTreeToChop);
        }
    }

    protected virtual bool NeedMoreTree()
    {
        return this.treeMax>=this.trees.Count;
    }

    protected virtual void PlantTree(WorkerCtrl workerCtrl)
    {
        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null) target = this.GetPlantPlace();
        if (target == null) return;

        workerCtrl.workerTasks.taskWorking.GoOutBuilding();
        workerCtrl.workerMovement.SetTarget(target);

        if (workerCtrl.workerMovement.IsCloseToTarget())
        {
            

            this.Planting(workerCtrl.transform);
            workerCtrl.workerMovement.SetTarget(null);
            Destroy(target.gameObject);
            if (!this.NeedMoreTree())
            {
                workerCtrl.workerTasks.TaskCurrentDone();
                workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
            }
        }
    }

    protected virtual void Planting(Transform trans)
    {
        GameObject treePrefab = this.GettreePrefab();
        GameObject treeObj = Instantiate<GameObject>(treePrefab);
        treeObj.transform.position = trans.position;
        treeObj.transform.rotation = trans.rotation;
        this.trees.Add(treeObj);
        TreeManager.Instance.TreeAdd(treeObj);
    }

    protected virtual GameObject GettreePrefab()
    {
        int rand = Random.Range(0, this.treePrefabs.Count);
        return this.treePrefabs[rand];
    }

    protected virtual Transform GetPlantPlace()
    {

        Vector3 newTreePos = this.RandomPlaceForTree();
        float dis = Vector3.Distance(transform.position, newTreePos);
        if (dis < this.treeDistance)
        {
            Debug.Log("GetPlantPlace Destroy GameObject", gameObject);
            return null;
        }

        GameObject treePlace = Instantiate(this.plantTreeObj);
        treePlace.transform.position = newTreePos;

        return treePlace.transform;
    }

    protected virtual Vector3 RandomPlaceForTree()
    {
        Vector3 positon = transform.position;
        positon.x += Random.Range(this.treeRange * -1, this.treeRange);
        positon.y = 0;
        positon.z += Random.Range(this.treeRange * -1, this.treeRange);

        return positon;
    }

    protected virtual void LoadNearByTree()
    {
        List<GameObject> allTrees = TreeManager.Instance.Trees();
        float dis;
        foreach (GameObject tree in allTrees)
        {
            dis = Vector3.Distance(tree.transform.position, transform.position);
            if (dis > this.treeDistance) continue;
            this.trees.Add(tree);

        }
    }

    public virtual void TreeAdd(GameObject tree)
    {
        if(this.trees.Contains(tree)) return;
        this.trees.Add(tree);
    }

    public virtual void ChopTree(WorkerCtrl workerCtrl)
    {
        if(workerCtrl.workerMovement.IsWorking) return;
        workerCtrl.workerMovement.IsWorking = true;
        StartCoroutine(Chopping(workerCtrl, workerCtrl.workerTasks.taskTarget));
    }

    IEnumerator Chopping(WorkerCtrl workerCtrl, Transform tree)
    {
        Debug.Log("Chopping Tree", gameObject);
        yield return new WaitForSeconds(this.chopSpeed);
        Debug.Log("Chopping Yield", gameObject);

        TreeCtrl treeCtrl = tree.GetComponent<TreeCtrl>();
        treeCtrl.treeLevel.ShowLastBuild();
        treeCtrl.logwoodGenerator.TakeAll(ResourceName.logwood);
        treeCtrl.choper = null;
        this.trees.Remove(treeCtrl.gameObject);
        TreeManager.Instance.Trees().Remove(treeCtrl.gameObject);

        workerCtrl.workerMovement.IsWorking = false;
        workerCtrl.workerTasks.taskTarget = null;
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void FindTreeToChop(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse)workerTasks.taskWorking.GoOutBuilding();
        if (workerTasks.taskTarget == null)
        {
            this.FindNearestTree(workerCtrl);
        }
        else if (workerCtrl.workerMovement.TargetDistance() <=1.5)
        {
            workerCtrl.workerMovement.SetTarget(null);
            workerCtrl.workerTasks.TaskCurrentDone();
        }
    }

    protected virtual void FindNearestTree(WorkerCtrl workerCtrl)
    {
        foreach(GameObject tree in this.trees)
        {
            TreeCtrl treeCtrl = tree.GetComponent<TreeCtrl>();
            if (treeCtrl == null) continue;
            if (!treeCtrl.treeLevel.IsMaxLevel()) continue;
            if (treeCtrl.choper != null) continue;

            treeCtrl.choper = workerCtrl;
            workerCtrl.workerTasks.taskTarget = treeCtrl.transform;
            workerCtrl.workerMovement.SetTarget(treeCtrl.transform);
            return;
        }
    }

    protected virtual bool IsStoreFull()
    {
        return this.storeCurrent >= this.storeMax;
    }

}

