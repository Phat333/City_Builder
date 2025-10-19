using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public virtual void Build()
    {
        string buildName = transform.name.Replace("btn", "");
        BuildManager.instance.CurrentBuildSet(buildName);
    }

    // Update is called once per frame
    public virtual void BuildClear()
    {
        BuildManager.instance.CurrentBuildClear();
    }
}
