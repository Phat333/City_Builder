using UnityEngine;

public class BuildDestroyTree : BuildDestroyable
{
    public override void Destroy()
    {
        TreeManager.Instance.TreeRemove(this.gameObject);
        base.Destroy();
    }
}
