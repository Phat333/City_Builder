using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResNameParser
{
    public static ResourceName FromString(string name)
    {
        return(ResourceName)Enum.Parse(typeof(ResourceName), name);
    }
}
public enum ResourceName
{
    noResource = 0,

    //Money
    gold = 1,

    //Materials Level 1
    water = 1000,
    wood = 1001,
    ironOre = 1002,

    //Materials Level 2
    blank = 2001,
    ironIngot = 2002,

}
