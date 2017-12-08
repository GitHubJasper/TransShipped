﻿using System.Collections.Generic;
using UnityEngine;

public abstract class DeliveryVehicle : MonoBehaviour
{
    public List<MonoContainer> carrying;
    private List<Container> outgoing = new List<Container>();


    protected readonly Queue<Vector3> movementQueue = new Queue<Vector3>();

    protected Vector3 destPos;
    protected Vector3 spawnPos;
    protected Vector3 spawnScale;
    protected Vector3 interPos;

    protected float height = 0.0f;

    protected float speed = 20.0f;

    public List<Container> Outgoing
    {
        get { return outgoing; }
        set { outgoing = value; }
    }

    protected Vector3 getNextPos()
    {
        float step = speed * Time.deltaTime;
        Vector3 tempTarget = movementQueue.Peek();
        return Vector3.MoveTowards(transform.position, tempTarget, step);
    }

    public void EnterTerminal()
    {
        interPos.x = destPos.x;
        interPos.y = height;
        interPos.z = spawnPos.z;

        movementQueue.Enqueue(interPos);
        movementQueue.Enqueue(destPos);
    }

    protected bool isAtDest()
    {
        return Vector3.Distance(destPos, transform.position) < speed * Time.deltaTime;
    }

    public void LeaveTerminal()
    {
        interPos.x = destPos.x;
        interPos.y = height;
        interPos.z = spawnPos.z;

        movementQueue.Enqueue(interPos);
        movementQueue.Enqueue(spawnPos);
    }

    protected void GenerateContainers(int from, int to)
    {
        var rnd = new System.Random();
        var conCount = rnd.Next(10, 30);

        for (var i = 0; i < conCount; ++i)
        {
            var temp = Instantiate(Resources.Load("Container") as GameObject, transform.position, transform.rotation)
                .GetComponent<MonoContainer>();
            carrying.Add(temp);
        }
    }
}