using System.Collections.Generic;

public class Truck : DeliveryVehicle {
    public TruckArea area;

    public override void LeaveTerminal() {
        MOPushDestination(truckSpawnPos);
    }

    private void Awake() {
        if (!Game.instance.trucks.Contains(this)) Game.instance.trucks.Add(this);
    }

    public void OnSelected() {
        List<TruckArea> areas = Game.instance.OnlyHighlight<TruckArea>();
        if (areas.Count == 1) {
            CommandPanel commandPanel = FindObjectOfType<CommandPanel>();
            if (!areas[0].occupied) {
                commandPanel.SetDeliveryArea(areas[0]);
                area = areas[0];
                areaPos = area.transform.position;
            }

            areas[0].Highlight(false);
            return;
        }

        foreach (TruckArea currentArea in areas)
            if (currentArea.occupied)
                currentArea.Highlight(false);
    }

    private void Start() {
        MOInit(truckSpawnPos, 20, false);
    }

    protected override void DestroyIfDone() {
        if (isAtDestination && MOIsAtTheThisPos(truckSpawnPos)) Destroy(gameObject);
    }

    protected override void Enter() {
        area.OnVehicleEnter(this);
    }
}