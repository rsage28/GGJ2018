using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : ScriptableObject {
	[SerializeField]
	private float moneyChange;
	[SerializeField]
	private float employeeChange;
	[SerializeField]
	private float listenerChange;
	[SerializeField]
	private float cultistChange;
	[SerializeField]
	private bool causesStationDestruction;

	[SerializeField]
	private float musicEffectivenessPlus;
	[SerializeField]
	private float adEffectivenessPlus;
	[SerializeField]
	private float convertEffectivenessPlus;

	[SerializeField]
	private float musicEffectivenessMult;
	[SerializeField]
	private float adEffectivenessMult;
	[SerializeField]
	private float convertEffectivenessMult;

	[SerializeField]
	private MusicType forcedMusicType;
	[SerializeField]
	private AdType forcedAdType;
	[SerializeField]
	private CultMessageType forcedCultMessageType;

	[SerializeField]
	private int towerMinRequirement = 1;
	[SerializeField]
	private int moneyMinRequirement = 0;

	[SerializeField]
	private int suspicionMinRequirement = 0;
	[SerializeField]
	private int localListenerMinRequirement = 0;
	[SerializeField]
	private int globalListenerMinRequirement = 0;
	[SerializeField]
	private int localCultistMinRequirement = 0;
	[SerializeField]
	private int globalCultistMinRequirement = 0;
	[SerializeField]
	private int suspicionMaxRequirement = int.MaxValue;
	[SerializeField]
	private int localListenerMaxRequirement = int.MaxValue;
	[SerializeField]
	private int globalListenerMaxRequirement = int.MaxValue;
	[SerializeField]
	private int localCultistMaxRequirement = int.MaxValue;
	[SerializeField]
	private int globalCultistMaxRequirement = int.MaxValue;

	[SerializeField]
	private bool needsTown;
	[SerializeField]
	private Town affectedTown;
	[SerializeField]
	private int duration = 0;
	[SerializeField]
	private string ongoingShortText;
	[SerializeField]
	private string eventText;
	[SerializeField]
	private List<EventButton> eventButtons;
	[SerializeField]
	private Event followupEvent;
	[SerializeField]
	private int followupDelayAfterDuration;

#region Effects
	public float MoneyChange {
		get { return moneyChange; }
		set { moneyChange = value; }
	}

	public float EmployeeChange {
		get { return employeeChange; }
		set { employeeChange = value; }
	}

	public float CultistChange {
		get { return cultistChange; }
		set { cultistChange = value; }
	}

	public float ListenerChange {
		get { return listenerChange; }
		set { listenerChange = value; }
	}

	public bool CausesStationDestruction {
		get { return causesStationDestruction; }
		set { causesStationDestruction = value; }
	}

	public float MusicEffectivenessPlus {
		get { return musicEffectivenessPlus; }
		set { musicEffectivenessPlus = value; }
	}
	public float AdEffectivenessPlus {
		get { return adEffectivenessPlus; }
		set { adEffectivenessPlus = value; }
	}
	public float ConvertEffectivenessPlus {
		get { return convertEffectivenessPlus; }
		set { convertEffectivenessPlus = value; }
	}
	public float MusicEffectivenessMult {
		get { return musicEffectivenessMult; }
		set { musicEffectivenessMult = value; }
	}
	public float AdEffectivenessMult {
		get { return adEffectivenessMult; }
		set { adEffectivenessMult = value; }
	}
	public float ConvertEffectivenessMult {
		get { return convertEffectivenessMult; }
		set { convertEffectivenessMult = value; }
	}
#endregion
#region Requirements
	public int TowerMinRequirement {
		get { return towerMinRequirement; }
		set { towerMinRequirement = value; }
	}

	public int MoneyMinRequirement {
		get { return moneyMinRequirement; }
		set { moneyMinRequirement = value; }
	}

	public int SuspicionMinRequirement {
		get { return suspicionMinRequirement; }
		set { suspicionMinRequirement = value; }
	}

	public int LocalListenerMinRequirement {
		get { return localListenerMinRequirement; }
		set { localListenerMinRequirement = value; }
	}

	public int GlobalListenerMinRequirement {
		get { return globalListenerMinRequirement; }
		set { globalListenerMinRequirement = value; }
	}

	public int LocalCultistMinRequirement {
		get { return localCultistMinRequirement; }
		set { localCultistMinRequirement = value; }
	}

	public int GlobalCultistMinRequirement {
		get { return globalCultistMinRequirement; }
		set { globalCultistMinRequirement = value; }
	}

	public int SuspicionMaxRequirement {
		get { return suspicionMaxRequirement; }
		set { suspicionMaxRequirement = value; }
	}

	public int LocalListenerMaxRequirement {
		get { return localListenerMaxRequirement; }
		set { localListenerMaxRequirement = value; }
	}

	public int GlobalListenerMaxRequirement {
		get { return globalListenerMaxRequirement; }
		set { globalListenerMaxRequirement = value; }
	}

	public int LocalCultistMaxRequirement {
		get { return localCultistMaxRequirement; }
		set { localCultistMaxRequirement = value; }
	}

	public int GlobalCultistMaxRequirement {
		get { return globalCultistMaxRequirement; }
		set { globalCultistMaxRequirement = value; }
	}
#endregion
#region General Settings
	public bool NeedsTown {
		get { return needsTown; }
		set { needsTown = value; }
	}

	public Town AffectedTown {
		get { return affectedTown; }
		set { affectedTown = value;}
	}

	public int Duration {
		get { return duration; }
		set { duration = value; }
	}

	public string OngoingShortText {
		get { return ongoingShortText; }
		set { ongoingShortText = value; }
	}

	public string EventText {
		get { return eventText; }
		set { eventText = value; }
	}

	public List<EventButton> EventButtons {
		get { return eventButtons; }
		set { eventButtons = value; }
	}

	public Event FollowupEvent {
		get { return followupEvent; }
		set { followupEvent = value; }
	}

	public int FollowupDelayAfterDuration {
		get { return followupDelayAfterDuration; }
		set { followupDelayAfterDuration = value;; }
	}
#endregion

	public Event() {}

	public Event(string eventText) {
		this.EventText = eventText;
	}

	public Event(string eventText, string ongoignShortText) {
		this.EventText = eventText;
		this.OngoingShortText = ongoignShortText;
	}
}
