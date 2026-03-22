using UnityEngine;

public class BuildDestroyable : MyBehaviour
{
    public virtual void Destroy()
    {
        PrefabManager.instance.Destroy(transform);
    }
}
