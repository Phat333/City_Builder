using System.Globalization;
using UnityEngine;

public class BuildForestHut : Buildbuilding
{
    protected override void LoadResRequires()
    {
        if (this.resRequires.Count > 0) return;
        this.resRequires.Add(new Resource { name = ResourceName.plank, number = 2 });
        Debug.Log(transform.name + ": LoadResRequires", gameObject);
    }
}
