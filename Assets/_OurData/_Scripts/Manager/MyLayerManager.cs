using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLayerManager : MyBehaviour
{
    public static MyLayerManager instance;

    [Header("Layers")]
    public int layerGround;
    public int layerBuilding;
    public int layerTree;

    protected override void Awake()
    {
        if (MyLayerManager.instance != null) Debug.LogError("Only 1 MyLayerManager allow");
        MyLayerManager.instance = this;

        this.LoadComponents();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.GetPlayers();
    }

    protected virtual void GetPlayers()
    {
        this.layerGround = LayerMask.NameToLayer("Ground");
        this.layerBuilding = LayerMask.NameToLayer("Building");
        this.layerTree = LayerMask.NameToLayer("Tree");

        if (this.layerGround < 0) Debug.LogError("Layer Ground is missing");
        if (this.layerBuilding < 0) Debug.LogError("Layer Building is missing");
        if (this.layerTree < 0) Debug.LogError("Layer Tree is missing");


        Debug.Log(transform.name + ": GetPlayers", gameObject);
    }

}
