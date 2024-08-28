using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Building: MonoBehaviour
{
    public int XLength { get; protected set; }
    public int YLength { get; protected set; }
    public int ZLength { get; protected set; }
    public float BaseCost { get; protected set; }
    public float MaintenanceCost { get; protected set; }

    protected Building(int xLength, int yLength, int zLength, float baseCost, float maintenanceCost)
    {
        XLength = xLength;
        YLength = yLength;
        ZLength = zLength;
        BaseCost = baseCost;
        MaintenanceCost = maintenanceCost;
    }
}
