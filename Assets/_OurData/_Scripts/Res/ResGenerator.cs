using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResGenerator : WareHouse
{
    [SerializeField] protected List<Resource> resCreate;
    [SerializeField] protected List<Resource> resRequire;
    public bool canCreate = true;
    [SerializeField] protected float createTime = 0f;
    [SerializeField] protected float createDelay = 7f;

    protected override void FixedUpdate()
    {
        this.Creating();
    }

    

    protected virtual void Creating()
    {
        if (!this.canCreate) return;
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

    public virtual bool IsAllResMax()
    {
        foreach(ResHolder resHolder in this.resHolders)
        {
            if (resHolder.IsMax() == false) return false;
        }

        return true;
    }

    public virtual List<Resource> TakeAll(ResourceName resourceName)
    {
        List<Resource> resources = new List<Resource>();
        foreach(ResHolder resHolder in this.resHolders)
        {
            Resource newResource = new Resource
            {
                name = resHolder.Name(),
                number = resHolder.TakeAll()
            };

            resources.Add(newResource);
        }

        return resources;
    }
}