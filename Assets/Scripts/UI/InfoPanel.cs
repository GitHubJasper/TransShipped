﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

    public Text targetText;
    public Text moneyText;
    public Text timeRemainingText;
    public Text indicatorText;

    // Use this for initialization
    void Start () {
        Game.instance.moneyChangeEvent += MoneyChanged;
        Game.instance.stateChangeEvent += StateChanged;
        Game.instance.stageChangeEvent += StageChanged;
        moneyText.text = "Money :" + Game.instance.money;

    }
	
	// Update is called once per frame
	void Update () {
	    if (!timeRemainingText.IsActive()) return;
	    DateTime start = ((OperationState) Game.instance.currentState).startTime;
	    int difference = (int) DateTime.Now.Subtract(start).TotalSeconds;
	    timeRemainingText.text = "Time left :" + (int) (Game.instance.currentStage.duration - difference);
	}

    public void MoneyChanged(double oldMoney, double newMoney) {
        moneyText.text = "Money :" + newMoney;
        indicatorText.GetComponent<Indic>().ShowLatestChange(newMoney - oldMoney);
    }

    public void StageChanged(Stage newStage) {
       targetText.text = "Target :" + newStage.moneyRequired;
    }

    public void StateChanged(GameState newState)
    {
        timeRemainingText.gameObject.SetActive(newState is OperationState);
    }
}
