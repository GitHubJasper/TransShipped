﻿using System.Collections.Generic;
using UnityEngine;

public abstract class DeliveryVehicle : MoveableObject {
    public Stack targetStack;
    public double reward;
    public double timeOutTime;
    public List<MonoContainer> carrying = new List<MonoContainer>();
    public List<Container> outgoing = new List<Container>();
    protected bool isAtDestination { get; set; }
    public Vector3 areaPos;
    protected static readonly Vector3 truckSpawnPos = new Vector3(-60, 0, 42);
    protected static readonly Vector3 trainSpawnPos = new Vector3(-60, 0, -40);
    protected static readonly Vector3 shipSpawnPos = new Vector3(65, 0, 35);

    public void EnterTerminal() {
        if (GetType() == typeof(Ship)) {
            Game.instance.ships.Remove(this as Ship);
            MOShipEnterTerminal(areaPos);
        } else if (GetType() == typeof(Truck)) {
            Game.instance.trucks.Remove(this as Truck);
            MOPushDestination(areaPos);
        } else {
            Game.instance.trains.Remove(this as Train);
            MOPushDestination(areaPos);
        }
    }

    public abstract void LeaveTerminal();

    protected abstract void DestroyIfDone();

    protected abstract void Enter();

    private void Update() {
        DestroyIfDone();
        if (!(Game.instance.currentState is OperationState)) return;
        MOMovementUpdate();

        if (isAtDestination || !MOIsAtTheThisPos(areaPos)) return;
        isAtDestination = true;

        Enter();
    }
}