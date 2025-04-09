using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResGenerator : WareHouse
{
    [SerializeField] protected List<Resource> resCreate;
    [SerializeField] protected List<Resource> resRequire;
    [SerializeField] protected float createTime = 5f;
    [SerializeField] protected float createDelay = 20f;

    protected override void FixedUpdate()
    {
        this.Creating();
    }

    

    protected virtual void Creating()
    {
        this.createTime += Time.fixedDeltaTime;
        if (this.createTime < this.createDelay) return;
        this.createTime = 0;

        if (!this.IsRequireEnough()) return;
        foreach(Resource res in this.resCreate)
        {
            //ResHolder resHolder = this.resHolders.Find((holder) => holder.Name() == res.name);
            ResHolder resHolder = this.GetHolder(res.name);
            resHolder.Add(res.number);
        }
    }

    protected virtual bool IsRequireEnough()
    {
        if (this.resRequire.Count < 1) return true;
        return false;
    }
    

    public virtual float GetCreateDelay()
    {
        return this.createDelay;
    }
}