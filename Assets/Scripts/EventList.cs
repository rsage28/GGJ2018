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
		events = new Dictionary<Event, int>();
		addDamagedTowerEvent();
		addCultistDamagedTowerEvent();
	}

	private void addDamagedTowerEvent() {
		Event damagedTower = ScriptableObject.CreateInstance<Event>();;
		damagedTower.EventText = "Your radio tower at "+damagedTower.AffectedTown+" was damaged in a storm and is now only operating at 50% effectiveness. Do you want to pay to fix it?";
		damagedTower.NeedsTown = true;
		damagedTower.TowerMinRequirement = 2;
		damagedTower.MoneyMinRequirement = 20000;
		damagedTower.LocalCultistMaxRequirement = 0;


		EventButton yesButton = ScriptableObject.CreateInstance<EventButton>();
		yesButton.Text = "Yes (-"+damagedTower.MoneyMinRequirement+" dollars)";
		yesButton.Event = ScriptableObject.CreateInstance<Event>();
		yesButton.Event.EventText = "It wasn't cheap, but everything is fixed and your tower is back in peak operating condition.";
		yesButton.Event.MoneyChange = damagedTower.MoneyMinRequirement;
		

		EventButton noButton = ScriptableObject.CreateInstance<EventButton>();
		noButton.Text = "No";
		noButton.Event = ScriptableObject.CreateInstance<Event>();
		noButton.Event.EventText = "Ah well, it'll probably fix itself eventually.";
		noButton.Event.AdEffectivenessMult = 0.5f;
		noButton.Event.MusicEffectivenessMult = 0.5f;
		noButton.Event.ConvertEffectivenessMult = 0.5f;
		noButton.Event.Duration = 3;
		noButton.Event.FollowupEvent = ScriptableObject.CreateInstance<Event>();
		noButton.Event.FollowupEvent.EventText = "Turns out it's important to keep your stations in tip-top shape. The tower snapped and fell on the station, caving in the roof and squishing everyone inside. You're gonna have to buy a whole new station if you want back in here.";
		noButton.Event.FollowupEvent.CausesStationDestruction = true;

		damagedTower.EventButtons.Add(yesButton);
		damagedTower.EventButtons.Add(noButton);

		Events.Add(damagedTower, 10);
	}

	private void addCultistDamagedTowerEvent() {
		Event damagedTower = ScriptableObject.CreateInstance<Event>();
		damagedTower.EventText = "Your radio tower at "+damagedTower.AffectedTown+" was damaged in a storm and is now only operating at 50% effectiveness. Do you want to pay to fix it?";
		damagedTower.NeedsTown = true;
		damagedTower.TowerMinRequirement = 2;
		damagedTower.LocalCultistMinRequirement = 1;

		EventButton yesButton = ScriptableObject.CreateInstance<EventButton>();
		yesButton.Text = "Make a cultist do it (Free labour!)";
		yesButton.Event = ScriptableObject.CreateInstance<Event>();
		yesButton.Event.FollowupEvent = ScriptableObject.CreateInstance<Event>();
		yesButton.Event.FollowupEvent.EventText = "After fixing the tower the cultist slipped and fell, splattering his innards all over the street outside the station. Oh well.";
		yesButton.Event.FollowupEvent.CultistChange = -1;
		
		EventButton noButton = ScriptableObject.CreateInstance<EventButton>();
		noButton.Text = "No";
		noButton.Event = ScriptableObject.CreateInstance<Event>();
		noButton.Event.EventText = "Ah well, it'll probably fix itself eventually.";
		noButton.Event.AdEffectivenessMult = 0.5f;
		noButton.Event.MusicEffectivenessMult = 0.5f;
		noButton.Event.ConvertEffectivenessMult = 0.5f;
		noButton.Event.Duration = 3;
		noButton.Event.FollowupEvent = ScriptableObject.CreateInstance<Event>();
		noButton.Event.FollowupEvent.EventText = "Turns out it's important to keep your stations in tip-top shape. The tower snapped and fell on the station, caving in the roof and squishing everyone inside. You're gonna have to buy a whole new station if you want back in here.";
		noButton.Event.FollowupEvent.CausesStationDestruction = true;
		damagedTower.EventButtons.Add(yesButton);
		damagedTower.EventButtons.Add(noButton);

		Events.Add(damagedTower, 10);
	}
	
}
