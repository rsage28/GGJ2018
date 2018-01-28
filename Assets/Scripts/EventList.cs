using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventList : MonoBehaviour {
	private Dictionary<Event, int> events;

	public Dictionary<Event, int> Events {
		get { return events; }
		private set { events = value; }
	}

	void Start () {
		addDamagedTowerEvent();
		addCultistDamagedTowerEvent();
	}

	private void addDamagedTowerEvent() {
		Event damagedTower = new Event();
		damagedTower.EventText = "Your radio tower was damaged in a storm and is now only operating at 50% effectiveness. Do you want to pay to fix it?";
		damagedTower.TowerMinRequirement = 2;
		damagedTower.MoneyMinRequirement = 20000;
		damagedTower.LocalCultistMaxRequirement = 0;

		EventButton yesButton = new EventButton("Yes (-"+damagedTower.MoneyMinRequirement+" dollars)");
		yesButton.Event = new Event();
		yesButton.Event.MoneyChange = damagedTower.MoneyMinRequirement;
		
		EventButton noButton = new EventButton("No");
		noButton.Event = new Event();
		noButton.Event.AdEffectivenessMult = 0.5f;
		noButton.Event.MusicEffectivenessMult = 0.5f;
		noButton.Event.ConvertEffectivenessMult = 0.5f;
		noButton.Event.Duration = 3;
		noButton.Event.FollowupEvent = new Event();
		noButton.Event.FollowupEvent.CausesStationDestruction = true;

		damagedTower.EventButtons.Add(yesButton);
		damagedTower.EventButtons.Add(noButton);

		Events.Add(damagedTower, 10);
	}

	private void addCultistDamagedTowerEvent() {
		Event damagedTower = new Event();
		damagedTower.EventText = "Your radio tower was damaged in a storm and is now only operating at 50% effectiveness. Do you want to pay to fix it?";
		damagedTower.TowerMinRequirement = 2;
		damagedTower.LocalCultistMinRequirement = 1;

		EventButton yesButton = new EventButton();
		yesButton.Event = new Event();
		yesButton.Text = "Make a cultist do it (Free labour!)";
		yesButton.Event.FollowupEvent = new Event("After fixing the tower the cultist slipped and fell, splattering his innards all over the street outside the station. Oh well.");
		yesButton.Event.FollowupEvent.CultistChange = -1;
		
		EventButton noButton = new EventButton("No");
		noButton.Event = new Event();
		noButton.Event.AdEffectivenessMult = 0.5f;
		noButton.Event.MusicEffectivenessMult = 0.5f;
		noButton.Event.ConvertEffectivenessMult = 0.5f;
		noButton.Event.Duration = 3;
		noButton.Event.FollowupEvent = new Event();
		noButton.Event.FollowupEvent.CausesStationDestruction = true;
		damagedTower.EventButtons.Add(yesButton);
		damagedTower.EventButtons.Add(noButton);

		Events.Add(damagedTower, 10);
	}
}
