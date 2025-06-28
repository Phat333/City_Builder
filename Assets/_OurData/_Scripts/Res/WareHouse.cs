using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WareHouse : MyBehaviour
{
    public BuildingType buildingType;
    [SerializeField] protected List<ResHolder> resHolders;
    [SerializeField] protected bool isFull = false;

    protected override void LoadComponents()
    {
        this.LoadHolders();
    }
    protected virtual void LoadHolders()
    {
        if (this.resHolders.Count > 0) return;

        Transform res = transform.Find("Res");
        foreach (Transform resTran in res)
        {
            Debug.Log(resTran.name);
            ResHolder resHolder = resTran.GetComponent<ResHolder>();
            if (resHolder == null) continue;
            this.resHolders.Add(resHolder);
        }
        Debug.Log(transform.name + " LoadHolders");
    }
    public virtual ResHolder GetHolder(ResourceName name)
    {
        return this.resHolders.Find((holder) => holder.Name() == name);
    }

    public virtual void AddByList(List<Resource> addResources)
    {
        foreach (Resource addResource in addResources)
        {
            this.AddResource(addResource.name, addResource.number);
        }
    }

    public virtual ResHolder AddResource(ResourceName resourceName, float number)
    {
        ResHolder resHolder = this.GetHolder(resourceName);
        resHolder.Add(number);
        return resHolder;
    }

    public virtual bool IsFull()
    {
        foreach (ResHolder resHolder in this.resHolders)
        {
            if (!resHolder.IsMax()) return false;
        }
        return true;
    }
}
