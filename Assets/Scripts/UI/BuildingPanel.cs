﻿using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour {
    public delegate void BuildingPanelListener();

    private int buttonCount = 0;
    private Button buttonObject;
    private List<Transform> buttons = new List<Transform>();

    public Transform prefab;

    public GameObject selected;
    private Text text5, text2, text3, text4, buttonText;

    public static event BuildingPanelListener GameStarted;

    private void Awake() {
        text2 = transform.Find("Text2").gameObject.GetComponent<Text>();
        text3 = transform.Find("Text3").gameObject.GetComponent<Text>();
        text4 = transform.Find("Text4").gameObject.GetComponent<Text>();
        text5 = GameObject.Find("Text5").gameObject.GetComponent<Text>();
        buttonObject = transform.Find("PurchaseButton").GetComponent<Button>();
        buttonText = buttonObject.GetComponentInChildren<Text>();
    }

    private void OnEnable() {
        buttonText.text = "";
        text2.text = "";
        text3.text = "";
        text4.text = "";
        text5.text = "";
        buttonObject.gameObject.SetActive(false);
    }

    public void SelectOptionalArea(OptionalArea optionalArea, string objectname, string attribute) {
        buttonObject.onClick.RemoveAllListeners();
        buttonText.text = "Purchase";
        text2.text = "Price for this area: " + optionalArea.price;
        text3.text = "Capacity for this area: " + optionalArea.capacity;
        text4.text = attribute;
        text5.text = objectname;
        if (Game.instance.money >= optionalArea.price) {
            buttonObject.gameObject.SetActive(true);
            buttonObject.onClick.AddListener(optionalArea.BuyStack);
            buttonObject.onClick.AddListener(StackBought);
        }
        else buttonObject.gameObject.SetActive(false);
    }

    private void StackBought() {
        buttonObject.onClick.RemoveAllListeners();
        text2.text = "";
        buttonObject.gameObject.SetActive(false);
    }

    public void SelectCraneArea(CraneArea craneArea, string objectname, string attribute) {
        buttonObject.onClick.RemoveAllListeners();
        buttonText.text = "Buy a new Crane";
        text2.text = "Price for one crane: " + craneArea.priceForOneCrane;
        {
            text3.text = "Currently there is " + craneArea.cranes.Count + " crane";
            if (craneArea.cranes.Count > 1) text3.text += "s";
        }
        text4.text = attribute;
        text5.text = objectname;
        if (!craneArea.NoMoreSpace() && Game.instance.money >= craneArea.priceForOneCrane) {
            buttonObject.gameObject.SetActive(true);
            buttonObject.onClick.AddListener(craneArea.BuyCrane);
            buttonObject.onClick.AddListener(() => SelectCraneArea(craneArea, objectname, attribute));
        }
        else {
            buttonObject.gameObject.SetActive(false);
            text3.text = "There is " + craneArea.cranes.Count + " cranes and no more could be bought";
        }
    }

    public void beginGame() {
        if (GameStarted != null) GameStarted.Invoke();

        selected = null;
        Game.instance.ChangeState(new OperationState());
    }

    public void SelectCrane(Crane crane) {
        buttonObject.onClick.RemoveAllListeners();
        buttonText.text = "Upgrade";
        text2.text = "Current Level: " + crane.Level;
        text4.text = "Current Efficiency: " + crane.speed;
        text5.text = crane.GetType().ToString();
        if (!crane.IsFullyUpgraded() && Game.instance.money >= crane.CostOfUpgrade) {
            text3.text = "Price for Upgrading: " + crane.CostOfUpgrade;
            buttonObject.gameObject.SetActive(true);
            buttonObject.onClick.AddListener(crane.Upgrade);
            buttonObject.onClick.AddListener(() => SelectCrane(crane));
        }
        else {
            buttonObject.gameObject.SetActive(false);
            text3.text = "This crane has already been fully upgraded";
        }
    }
}