using System.Collections.Generic;
using UnityEngine;

public enum VehicleType{Ship = 0, Truck = 1, Train = 2}

[CreateAssetMenu(menuName = "VehicleTemplate")]
public class VehicleTemplate : ScriptableObject
{
    public VehicleType type;
    public Transform prefab;
    public List<containerType> carryingTypes;
    public List<containerType> requestTypes;
    public int carryMin;
    public int carryMax;
    public int requestMin;
    public int requestMax;
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    public double reward;
    public double timeOutTime;
}