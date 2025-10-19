using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreeManager : MyBehaviour
{
    public static TreeManager Instance;
    [SerializeField] protected List<GameObject> trees;

    protected override void Awake()
    {
        base.Awake();
        if (TreeManager.Instance != null) Debug.LogError("Only 1 TreeManager allow");
        TreeManager.Instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTrees();
    }

    protected virtual void LoadTrees()
    {
        if (this.trees.Count > 0) return;
        foreach(Transform tree in transform)
        {
            this.trees.Add(tree.gameObject);
        }

        Debug.Log(transform.name + " Load Trees", gameObject);
    }

    public virtual void TreeAdd(GameObject tree)
    {
        if (this.trees.Contains(tree)) return;

        this.trees.Add(tree);
        tree.transform.parent = transform;
    }

    public virtual List<GameObject> Trees()
    {
        return this.trees;
    }
}
