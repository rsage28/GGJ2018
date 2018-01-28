using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButton : ScriptableObject {
	[SerializeField]
	private string text;
	[SerializeField]
	private Event buttonEvent;

	public string Text {
		get { return text; }
		set { text = value; }
	}

	public Event Event {
		get { return buttonEvent; }
		set { buttonEvent = value; }
	}
}
