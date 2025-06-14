using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLevel : BuildLevel
{
    [SerializeField] protected bool isMaxLevel = false;
    [SerializeField] protected LogwoodGenerator tree;
    [SerializeField] protected float treeTimer = 0f;
    [SerializeField] protected float treeDelay = Mathf.Infinity;


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.Growing();
        this.IsMaxLevel();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTree();
    }

    protected virtual void LoadTree()
    {
        if(this.tree != null) return;
        this.tree = this.GetComponent<LogwoodGenerator>();

        this.GetTreeDelay();

        Debug.Log(transform.name + "LoadTree");
    }
    protected virtual void GetTreeDelay()
    {
        int levelCount = this.levels.Count - 2;
        if (this.tree == null)
        {
            Debug.LogError($"{transform.name} - TreeLevel: `tree` is null!", this);
            this.treeDelay = 0;
            return;
        }
        this.treeDelay = this.tree.GetCreateDelay() / levelCount;
    }
    protected virtual void Growing()
    {
        this.treeTimer += Time.fixedDeltaTime;
        if (this.treeTimer < this.treeDelay) return;
        this.treeTimer = 0;
        this.ShowNextBuild();
    }

    public virtual bool IsMaxLevel()
    {
        if (this.currentLevel == this.levels.Count - 2) this.isMaxLevel = true;
        else this.isMaxLevel = false;

        return this.isMaxLevel;
    }
}
