﻿using System.Collections.Generic;
using UnityEngine;

public class Stack : Area {
    public List<MonoContainer> containers = new List<MonoContainer>();

    private CapacitySlider script;

    public int max;

    public GameObject slider;
    GameObject sliderclone;

    public int Contains(Container container) {
        for (var i = 0; i < containers.Count; i++) {
            if (containers[i].container.Equals(container)) {
                return i;
            }
        }

        return -1;
    }

    public void Update() {
        for (var i = containers.Count - 1; i >= 0; i--) {
            if (MoveToNext(containers[i])) {
                break;
            }

            var n = i % (max / 5);
            containers[i].transform.position = new Vector3(
                transform.position.x - transform.lossyScale.x / 2 + 2 + 2 * (int) (n / (transform.lossyScale.z - 2)),
                0.5f + i * 5 / max,
                transform.position.z - transform.lossyScale.z / 2 + 1.5f + n % (transform.lossyScale.z - 2));
        }
    }

    public override bool AddContainer(MonoContainer monoContainer) {
        if (containers.Count >= max) return false;
        containers.Add(monoContainer);
        if (monoContainer.movement.TargetArea == this) {
            monoContainer.movement = null;
        }

        if (script != null) {
            script.ChangeSliderValue((float) containers.Count / max);
        }

        return true;
    }

    protected override void RemoveContainer(MonoContainer monoContainer) {
        containers.Remove(monoContainer);
    }

    private void OnMouseDown() {
        if (highlight && Game.instance.currentState is OperationState) {
            CommandPanel commandPanel = FindObjectOfType<CommandPanel>();
            commandPanel.SetStackArea(this);
        }
    }

    private void OnMouseEnter() {
        if (!(Game.instance.currentState is OperationState)) return;
        sliderclone = Instantiate(slider, transform.position, Quaternion.identity);

        sliderclone.transform.SetParent(GameObject.Find("Canvas").transform, false);

        //sliderclone.transform.parent = GameObject.Find("Canvas").transform;
        script = sliderclone.GetComponent<CapacitySlider>();
        script.target = transform;
        script.ChangeSliderValue((float) containers.Count / max);
        GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void OnMouseExit() {
        if (!(Game.instance.currentState is OperationState)) return;
        Destroy(sliderclone);
        if (highlight) {
            GetComponent<Renderer>().material = highlightMat;
        }
        else {
            GetComponent<Renderer>().material = defaultMat;
        }
    }
}