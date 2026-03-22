using System.Globalization;
using UnityEngine;

public class BuildHouseBuilder : Buildbuilding
{
    protected override void LoadResRequires()
    {
        if (this.resRequires.Count > 0) return;
        this.resRequires.Add(new Resource { name = ResourceName.logwood, number = 5 });
        Debug.Log(transform.name + ": LoadResRequires", gameObject);
    }
    
}
