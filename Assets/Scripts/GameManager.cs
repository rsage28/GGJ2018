﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {
    public static UnityEvent TimeTick;

    private static GameManager instance;

    [SerializeField]
    private GameCamera gameCamera;
    [SerializeField]
    private Town selectedTown;

    private List<Town> towns;

    private float timeScale;
    private float nextTimeStep;

    [SerializeField]
    private float listenerCount = 0;
    [SerializeField]
    private float cultistCount = 0;
    [SerializeField]
    private float moneyCount = 20000;
    [SerializeField]
    private float eventChance = 15;
    [SerializeField]
    private float suspicionLevel = 0;
    [SerializeField]
    private float timeStep = 60;

    // HUD text
    public Text listenerCountText;
    public Text moneyCountText;
    public Text cultistCountText;
    public Text suspicionLevelText;

    // Town info text
    public Text populationCountText;
    public Text listenerCountTextTown;
    public Text cultistCountTextTown;
    public Text convertabilityLevelText;
    public Text preferredMusicTypeText;
    public Text preferredAdTypeText;
    public Text preferredCultTypeText;
    public Text stationCostText;

    public Canvas HUD;
    public Canvas stationManager;
    public Canvas townInfo;

    public Station stationClone;

    public static GameManager Instance {
        get { return instance; }
        private set {
            if (instance == null) {
                instance = value;
            }
        }
    }

    public GameCamera GameCamera {
        get { return gameCamera; }
        private set { gameCamera = value; }
    }

    public Town SelectedTown {
        get { return selectedTown; }
        set { selectedTown = value; }
    }

    public List<Town> Towns {
        get { return towns; }
        private set { towns = value; }
    }

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

    void Start() {
        nextTimeStep = Time.time + TimeStep;
        Towns = FindObjectsOfType<Town>().ToList();
        UpdateText();
    }

    void Awake() {
        GameCamera = FindObjectOfType<GameCamera>();
        Instance = this;
        if (TimeTick == null) {
            TimeTick = new UnityEvent();
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Time.timeScale = 0;
        }

        if (Time.time >= nextTimeStep) {
            nextTimeStep = Time.time + TimeStep;
            TimeTick.Invoke();
            if (Random.Range(0, 100) <= EventChance) {
                RandomEvent();
            }
            UpdateText();
        }
    }

    private void RandomEvent() {
        print("a random event happened");
    }
    
    void UpdateText() {
        listenerCountText.text = "Listeners: " + ListenerCount;
        moneyCountText.text = "Money: $" + MoneyCount;
        cultistCountText.text = "Cult Followers: " + CultistCount;
        suspicionLevelText.text = "Suspicion Level: " + SuspicionLevel;
    }

    public Town getNearestTown(Vector3 pos) {
        return Towns.OrderBy(t => (t.transform.position - pos).sqrMagnitude).FirstOrDefault();
    }

    public void SelectTown(Town town) {
        SelectedTown = town;
        townInfo.enabled = town != null;
        if (townInfo.enabled) {
            populationCountText.text = "Town Population: " + SelectedTown.Population;
            listenerCountTextTown.text = "Listener Count: " + SelectedTown.Listeners;
            cultistCountTextTown.text = "Cultist Count: " + SelectedTown.Cultists;
            convertabilityLevelText.text = "Convertability Level: " + SelectedTown.Convertability + "%";
            preferredMusicTypeText.text = "Preferred Music Type: " + SelectedTown.PreferredMusicType.ToString();
            preferredAdTypeText.text = "Prefered Ad Type: " + SelectedTown.PreferredAdType.ToString();
            preferredCultTypeText.text = "Preferred Cult Type: " + SelectedTown.PreferredCultMessageType.ToString();
            stationCostText.text = "Station Cost: ";
            SelectRadioStation(SelectedTown.ContainedStation);
        }        
    }

    private void SelectRadioStation(Station station) {
        stationManager.enabled = station != null;
        if (stationManager.enabled) {

        }
    }

    public void BuyStation() {
        Station newStation = Instantiate(stationClone, SelectedTown.transform);
        SelectedTown.ContainedStation = newStation;
        newStation.ContainingTown = SelectedTown;
    }
}
