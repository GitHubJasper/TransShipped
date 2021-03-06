﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void OnStateChanged(GameState newState);

public delegate void OnStageChanged(Stage newStage);

public delegate void OnMoneyChanged(double oldValue, double newValue);

public class Game : MonoBehaviour {
    static Game _instance;// only one Go can exist
	public static Game instance
	{
		get
		{
			if( !_instance)
			{
				_instance = FindObjectOfType<Game>();
			}

			return _instance;
		}
	}
    public GameState currentState { get; private set; }
    public Stage currentStage;
    public List<Stage> stagesList;
    public List<HighlightAble> highlights = new List<HighlightAble>();
    public Queue<Stage> stages;
    public int movements;
    public double startMoney;

    private double moneyValue;

    public double money {
        get { return moneyValue; }
        set {
            if (instance.moneyChangeEvent != null && moneyValue != value) instance.moneyChangeEvent(moneyValue, value);

            moneyValue = value;
        }
    }

    public List<Ship> ships { get; private set; }
    public List<Truck> trucks { get; private set; }
    public List<Train> trains { get; private set; }
    private readonly List<OptionalArea> optionalAreas = new List<OptionalArea>();
    private readonly List<Area> areas = new List<Area>();
    public event OnStateChanged stateChangeEvent;
    public event OnStageChanged stageChangeEvent;
    public event OnMoneyChanged moneyChangeEvent;

    public void Awake() {
        if (_instance == null)
            _instance = this;
        else if (instance != this)
            _instance = this;
        
    }

    public void Start() {
        money = startMoney;
        stages = new Queue<Stage>(stagesList);
        if (stages.Count > 0)
            SetStage(stages.Dequeue());
        else
            ChangeState(new LevelEndState());

        ChangeState(new UpgradeState());
        ships = new List<Ship>();
        trucks = new List<Truck>();
        trains = new List<Train>();
    }

    public void Update() {
        currentState.Update();
    }

    public void RegisterArea(Area area) {
        if (!instance.areas.Contains(area)) instance.areas.Add(area);
    }

    public void RegisterHighlight(HighlightAble highlight) {
        if (!instance.highlights.Contains(highlight)) instance.highlights.Add(highlight);
    }

    public void DeregisterHighlight(HighlightAble highlight) {
        if (instance.highlights.Contains(highlight)) instance.highlights.Remove(highlight);
    }

    public void RegisterArea(OptionalArea area) {
        if (!instance.optionalAreas.Contains(area)) instance.optionalAreas.Add(area);
    }

    public void DeregisterArea(OptionalArea area) {
        if (instance.optionalAreas.Contains(area)) instance.optionalAreas.Remove(area);
    }

    public List<T> GetAreasOfType<T>() where T : Area {
        return instance.areas.OfType<T>().Select(a => a).ToList();
    }

    public List<T> OnlyHighlight<T>() where T : Area {
        foreach (HighlightAble currentArea in instance.highlights) currentArea.Highlight(currentArea is T);

        return GetAreasOfType<T>();
    }

    public void ForceRemoveHighlights() {
        foreach (HighlightAble currentArea in instance.highlights) currentArea.ForceHighlight(false);
    }

    public void RemoveHighlights() {
        foreach (HighlightAble currentArea in instance.highlights) currentArea.Highlight(false);
    }

    public VehicleGenerator GetGenerator() {
        OperationState state = instance.currentState as OperationState;
        return state != null ? state.generator : null;
    }

    public ContainerManager GetManager() {
        OperationState state = instance.currentState as OperationState;
        return state != null ? state.manager : null;
    }

    public void ChangeState(GameState newState) {
        if (stateChangeEvent != null) stateChangeEvent.Invoke(newState);

        currentState = newState;
    }

    public void SetStage(Stage newStage) {
        currentStage = newStage;
        if (stageChangeEvent != null) stageChangeEvent(newStage);
    }
}