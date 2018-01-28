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

	public EventButton() {}

	public EventButton(string text) {
		this.text = text;
	}

	public EventButton(string text, Event buttonEvent) {
		this.text = text;
		this.buttonEvent = buttonEvent;
	}
}
