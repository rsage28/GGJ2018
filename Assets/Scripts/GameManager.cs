using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private float timeScale;
    private float nextTimeStep;

    private float eventChance;
    private float listenerCount;
    private float cultistCount;
    private float moneyCount;
    private float suspicionLevel;
    [SerializeField]
    private float timeStep = 60;

    public Text listenerCountText;
    public Text moneyCountText;
    public Text cultistCountText;
    public Text suspicionLevelText;

    public static UnityEvent TimeTick;

    public float ListenerCount {
        get { return listenerCount; }
        set { listenerCount = value; }
    }

    public float CultistCount {
        get { return cultistCount; }
        set { cultistCount = value; }
    }

    public float MoneyCount {
        get { return moneyCount; }
        set { moneyCount = value; }
    }

    public float EventChance {
        get { return eventChance; }
        set { eventChance = value; }
    }

    public float SuspicionLevel {
        get { return suspicionLevel; }
        set { suspicionLevel = value; }
    }

    public float TimeStep {
        get { return timeStep; }
        set { timeStep = value; }
    }

    // Use this for initialization
    void Start() {
        nextTimeStep = Time.time + TimeStep;
        ListenerCount = 0f;
        CultistCount = 0f;
        MoneyCount = 20000f;
        EventChance = 15f;
        SuspicionLevel = 0f;
        UpdateText();
    }

    void Awake() {
        if (TimeTick == null) {
            TimeTick = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update() {
        ListenerCount += 10f;
        UpdateText();
        if (Time.time >= nextTimeStep) {
            nextTimeStep = Time.time + TimeStep;
            TimeTick.Invoke();
            if (Random.Range(0, 100) <= EventChance) {
                RandomEvent();
            }
            UpdateText();
        }        
    }

    void RandomEvent() {
        print("a random event happened");
    }

    void UpdateText() {
        listenerCountText.text = "Listeners: " + ListenerCount;
        moneyCountText.text = "Money: $" + MoneyCount;
        cultistCountText.text = "Cult Followers: " + CultistCount;
        suspicionLevelText.text = "Suspicion Level: " + SuspicionLevel;
    }
}
