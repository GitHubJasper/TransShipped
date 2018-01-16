﻿using System.Collections.Generic;
using UnityEngine;

public abstract class Area : MonoBehaviour
{
    public bool highlight = false;
    public Material defaultMat;
    public Material highlightMat;
    public List<Area> connected;
    public List<Area> listening = new List<Area>();

    private readonly Dictionary<Area, Queue<MonoContainer>> containerQueue =
        new Dictionary<Area, Queue<MonoContainer>>();

    
    public void Start()
    {
        if(this.GetComponent<Renderer>() != null){
            if(highlight){
                this.GetComponent<Renderer>().material = highlightMat;
            } else{
                this.GetComponent<Renderer>().material = defaultMat;
            }
        }
        Game.RegisterArea(this);
    }

    public void Highlight(bool highlight){
        this.highlight = highlight;
        if(this.GetComponent<Renderer>() != null){
            if(highlight) {
                this.GetComponent<Renderer>().material = highlightMat;
            } else{
                this.GetComponent<Renderer>().material = defaultMat;
            }
        }
    }

    public void Connect(Area connectArea)
    {
        connected.Add(connectArea);
        connectArea.connected.Add(this);
    }

    public virtual bool ReserveArea(Area origin, Movement movement)
    {
        return true;
    }

    private void AddListener(Area area)
    {
        if (!listening.Contains(area))
            listening.Add(area);
    }

    public void AreaAvailable(Area availableTo)
    {
        if (listening.Contains(availableTo))
            availableTo.OnAreaAvailable(this);
    }

    public void AreaAvailable()
    {
        for (var i = listening.Count - 1; i >= 0; i--)
            listening[i].OnAreaAvailable(this);
    }

    public void OnAreaAvailable(Area area)
    {
        Queue<MonoContainer> queue = containerQueue[area];
        MonoContainer container;
        if (queue.Count > 0)
            container = queue.Peek();
        else
            return;

        if (!MoveToNext(container)) return;
        queue.Dequeue();
        if (queue.Count == 0)
            area.listening.Remove(this);
    }

    public abstract bool AddContainer(MonoContainer monoContainer);

    protected abstract void RemoveContainer(MonoContainer monoCont);

    public bool MoveToNext(MonoContainer monoCont)
    {
        if (monoCont.movement == null || !(Game.instance.currentState is OperationState)) return false;
        var nextArea = ((OperationState) Game.instance.currentState).manager.GetNextArea(this, monoCont.movement);
        Transform previousParent = monoCont.transform.parent;
        monoCont.transform.SetParent(nextArea.transform);
        Area previousOrigin = monoCont.movement.originArea;
        monoCont.movement.originArea = this;
        if (!nextArea.AddContainer(monoCont))
        {
            monoCont.movement.originArea = previousOrigin;
            monoCont.transform.SetParent(previousParent);
            return false;
        }

        RemoveContainer(monoCont);
        return true;
    }

    protected void AddToQueue(MonoContainer monoCont)
    {
        if (monoCont.movement == null || !(Game.instance.currentState is OperationState)) return;
        Area nextArea = ((OperationState) Game.instance.currentState).manager.GetNextArea(this, monoCont.movement);
        if (!containerQueue.ContainsKey(nextArea))
            containerQueue.Add(nextArea, new Queue<MonoContainer>());
        if (!containerQueue[nextArea].Contains(monoCont))
            containerQueue[nextArea].Enqueue(monoCont);
        nextArea.AddListener(this);
    }
}