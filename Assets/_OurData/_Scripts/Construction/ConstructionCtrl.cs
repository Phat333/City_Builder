using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ConstructionCtrl : MyBehaviour
{
    public LimitRadius limitRadius;
    public AbstractConstruction abstractConstruction;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLimitRadius();
        this.LoadAbstractContruction();
    }
    protected virtual void LoadLimitRadius()
    {
        if (this.limitRadius != null) return;
        this.limitRadius = GetComponent<LimitRadius>();
        Debug.Log(transform.name + ": LoadLimitRadius", gameObject);
    }

    protected virtual void LoadAbstractContruction()
    {
        if (this.abstractConstruction != null) return;
        this.abstractConstruction = GetComponent<AbstractConstruction>();
        Debug.Log(transform.name + ": LoadAbstractContruction", gameObject);
    }

}
