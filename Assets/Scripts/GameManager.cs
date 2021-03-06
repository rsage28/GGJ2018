﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManager : MonoBehaviour {
    public static UnityEvent TimeTick;

    private static GameManager instance;

    [SerializeField]
    private GameCamera gameCamera;
    [SerializeField]
    private Town selectedTown;
    [SerializeField]
    private Station selectedStation;

    private List<Town> towns = new List<Town>();
    [SerializeField]
    private List<Town> unlockedTowns = new List<Town>();

    private float timeScale;
    private float nextTimeStep;

    [SerializeField]
    EventList eventList;
    [SerializeField]
    List<Event> ongoingEvents;

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
    public Text townName;
    public Text populationCountText;
    public Text listenerCountTextTown;
    public Text cultistCountTextTown;
    public Text convertabilityLevelText;
    public Text preferredMusicTypeText;
    public Text preferredAdTypeText;
    public Text preferredCultTypeText;
    public Text stationCostText;

    // Town Buttons
    public Button buyStationButton;

    // Station text
    public Text musicEffectivePercentText;
    public Text propEffectivePercentText;
    public Text employeeCountText;
    public Text cultistStationCountText;

    // Station Dropdowns
    public Dropdown musicTypes;
    public Dropdown adTypes;
    public Dropdown cultTypes;
    public Dropdown marketingTypes;

    // Station sliders
    public Slider musicTime;
    public Slider adTime;
    public Slider cultTime;
    public Slider moneyVsHappy;

    // Event Text
    public Text eventText;
    public Text buttonText1;
    public Text buttonText2;
    public Text buttonText3;

    // Event Buttons
    public Button button1;
    public Button button2;
    public Button button3;

    // Canvases
    public Canvas HUD;
    public Canvas stationManager;
    public Canvas townInfo;
    public Canvas eventUI;

    // Objects to create
    public Station stationClone;
    public Employee employeeClone;

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

    public Station SelectedStation {
        get { return selectedStation; }
        set { selectedStation = value; }
    }

    public List<Town> UnlockedTowns {
        get { return unlockedTowns; }
        set { unlockedTowns = value; }
    }

    void Start() {
        nextTimeStep = Time.time + TimeStep;
        Towns = FindObjectsOfType<Town>().ToList();
        UpdateText();
        PopulateMusicDropdown();
        PopulateAdDropdown();
        PopulateCultDropdown();
        PopulateMarketingDropdown();
    }

    void Awake() {
        GameCamera = FindObjectOfType<GameCamera>();
        Instance = this;
        if (TimeTick == null) {
            TimeTick = new UnityEvent();
        }
        eventList = gameObject.AddComponent<EventList>();
        ongoingEvents = new List<Event>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Time.timeScale = 0;
        }

        if (Time.time >= nextTimeStep) {
            nextTimeStep = Time.time + TimeStep;
            doOngoingEvents();
            if (UnityEngine.Random.Range(0, 100) <= EventChance) {
                randomEvent();
            }
            TimeTick.Invoke();
            UpdateText();
        }

        if (SelectedTown != null) {
            buyStationButton.interactable = (MoneyCount >= SelectedTown.StationCost && UnlockedTowns.Contains(SelectedTown));
        }
    }

    private void doOngoingEvents() {
        foreach (Event ongoingEvent in ongoingEvents) {
            ongoingEvent.Duration--;
            doEventEffects(ongoingEvent);
        }
    }

    private void doEventEffects(Event eventToDo) {
        MoneyCount += eventToDo.MoneyChange;

        if (eventToDo.AffectedTown != null) {
            if (eventToDo.CausesStationDestruction) {
                Destroy(eventToDo.AffectedTown.ContainedStation);
            }
            
            if (eventToDo.EmployeeChange > 0) {
                for (int i = 0; i < eventToDo.EmployeeChange; i++) {
                    HireEmployee(eventToDo.AffectedTown.ContainedStation);
                }
            } else if (eventToDo.EmployeeChange < 0) {
                for (int i = 0; i < eventToDo.EmployeeChange; i++) {
                    FireEmployee(eventToDo.AffectedTown.ContainedStation);
                }
            }
            
            if (eventToDo.ForcedMusicType != null) {
                eventToDo.AffectedTown.ContainedStation.ChosenMusicType = eventToDo.ForcedMusicType;
            }
            if (eventToDo.ForcedAdType != null) {
                eventToDo.AffectedTown.ContainedStation.ChosenAdType = eventToDo.ForcedAdType;
            }
            if (eventToDo.ForcedCultMessageType != null) {
                eventToDo.AffectedTown.ContainedStation.ChosenCultMessageType = eventToDo.ForcedCultMessageType;
            }

            eventToDo.AffectedTown.Listeners += eventToDo.ListenerChange;
            eventToDo.AffectedTown.Cultists += eventToDo.CultistChange;

            //eventToDo.MusicEffectivenessPlus
            //eventToDo.AdEffectivenessPlus
            //eventToDo.ConvertEffectivenessPlus
            //eventToDo.MusicEffectivenessMult
            //eventToDo.AdEffectivenessMult
            //eventToDo.ConvertEffectivenessMult
        }
    }

    private void randomEvent() {
        Event chosenEvent = null;
        int attempts = 0;
        int maxAttempts = 10;
        while (chosenEvent == null && attempts <= maxAttempts) {
            KeyValuePair<Event, int> randomEvent = eventList.Events.ElementAt(UnityEngine.Random.Range(0, eventList.Events.Count));
            maxAttempts++;
            if (UnityEngine.Random.Range(0, 100) <= randomEvent.Value && globalEventRequirementsMet(randomEvent.Key)) {
                if (randomEvent.Key.NeedsTown) {
                    Town affectedTown = getRandomTownForEvent(randomEvent.Key);
                    if (affectedTown != null) {
                        chosenEvent = randomEvent.Key;
                        chosenEvent.AffectedTown = affectedTown;
                        doEventEffects(chosenEvent);
                        if (chosenEvent.Duration > 0) {
                            ongoingEvents.Add(chosenEvent);
                        }
                    }
                }
            }
        }
        if (chosenEvent != null) {
            button1.enabled = false;
            button2.enabled = false;
            button3.enabled = false;
            Time.timeScale = 0;
            eventUI.enabled = true;
            eventText.text = chosenEvent.EventText;
            for (int i = 0; i < chosenEvent.EventButtons.Count; i++) {
                switch(i) {
                    case 0:
                        button1.enabled = true;
                        button1.onClick.AddListener(DismissEvent);
                        buttonText1.text = chosenEvent.EventButtons[i].Text; break;
                    case 1:
                        button2.enabled = true;
                        button2.onClick.AddListener(DismissEvent);
                        buttonText2.text = chosenEvent.EventButtons[i].Text; break;
                    case 2:
                        button3.enabled = true;
                        button3.onClick.AddListener(DismissEvent);
                        buttonText3.text = chosenEvent.EventButtons[i].Text; break;
                }
            }
        }
    }

    private void DismissEvent() {
        Time.timeScale = 1;
        eventUI.enabled = false;
    }

    private bool globalEventRequirementsMet(Event eventToCheck) {
        bool requirementsMet = true;
        //requirementsMet =  >= eventToCheck.towerMinRequirement;
        requirementsMet = moneyCount >= eventToCheck.MoneyMinRequirement;

        requirementsMet = suspicionLevel >= eventToCheck.SuspicionMinRequirement;
        requirementsMet = listenerCount >= eventToCheck.GlobalListenerMinRequirement;
        requirementsMet = cultistCount >= eventToCheck.GlobalCultistMinRequirement;
        requirementsMet = suspicionLevel <= eventToCheck.SuspicionMaxRequirement;
        requirementsMet = listenerCount <= eventToCheck.GlobalListenerMaxRequirement;
        requirementsMet = cultistCount <= eventToCheck.GlobalCultistMaxRequirement;

        return requirementsMet;
    }

    private Town getRandomTownForEvent(Event eventToCheck) {
        Town randomTown = null;
        int townAttempts = 0;
        int maxTownAttempts = UnlockedTowns.Count;

        while (eventToCheck.AffectedTown == null && townAttempts <= maxTownAttempts) {
            townAttempts++;
            randomTown = UnlockedTowns[UnityEngine.Random.Range(0, UnlockedTowns.Count)];
            if (randomTown.ContainedStation != null && localEventRequirementsMetByTown(eventToCheck, randomTown)) {
                return randomTown;
            }
        }
        return randomTown;
    }

    private bool localEventRequirementsMetByTown(Event eventToCheck, Town town) {
        bool requirementsMet = true;

        requirementsMet = town.Listeners >= eventToCheck.LocalListenerMinRequirement;
        requirementsMet = town.Cultists >= eventToCheck.LocalCultistMinRequirement;
        requirementsMet = town.Listeners <= eventToCheck.LocalListenerMaxRequirement;
        requirementsMet = town.Cultists <= eventToCheck.LocalCultistMaxRequirement;

        return requirementsMet;
    }
    
    void UpdateText() {
        listenerCountText.text = "Listeners: " + ListenerCount;
        moneyCountText.text = "Money: $" + MoneyCount;
        cultistCountText.text = "Cult Followers: " + CultistCount;
        suspicionLevelText.text = "Suspicion Level: " + SuspicionLevel;
        if (townInfo.enabled) {
            townName.text = "Town Name: " + SelectedTown.name;
            populationCountText.text = "Town Population: " + SelectedTown.Population;
            listenerCountTextTown.text = "Listener Count: " + SelectedTown.Listeners;
            cultistCountTextTown.text = "Cultist Count: " + SelectedTown.Cultists;
            convertabilityLevelText.text = "Convertability Level: " + SelectedTown.Convertability + "%";
            preferredMusicTypeText.text = "Preferred Music Type: " + SelectedTown.PreferredMusicType.ToString();
            preferredAdTypeText.text = "Prefered Ad Type: " + SelectedTown.PreferredAdType.ToString();
            preferredCultTypeText.text = "Preferred Cult Type: " + SelectedTown.PreferredCultMessageType.ToString();
            stationCostText.text = "Station Cost: " + SelectedTown.StationCost.ToString();
        }
        if (stationManager.enabled) {
            musicTypes.value = (int) SelectedStation.ChosenMusicType;
            adTypes.value = (int) SelectedStation.ChosenAdType;
            cultTypes.value = (int) SelectedStation.ChosenCultMessageType;
            marketingTypes.value = (int) SelectedStation.ChosenMarketingPlanType;
            musicEffectivePercentText.text = "Music Effectiveness: " + SelectedStation.MusicEffectivePercent.ToString() + "%";
            propEffectivePercentText.text = "Propaganda Effectiveness: " + SelectedStation.PropagandaEffectivePercent.ToString() + "%";
            employeeCountText.text = "Employee Count: " + SelectedStation.Employees.Count.ToString();
            musicTime.value = SelectedStation.MusicTimePercent;
            adTime.value = SelectedStation.AdTimePercent;
            cultTime.value = SelectedStation.CultTimePercent;
            moneyVsHappy.value = SelectedStation.MoneyVsHappiness;
        }
    }

    public Town getNearestTown(Vector3 pos) {
        return Towns.OrderBy(t => (t.transform.position - pos).sqrMagnitude).FirstOrDefault();
    }

    public void SelectTown(Town town) {
        SelectedTown = town;
        townInfo.enabled = town != null;
        if (townInfo.enabled) {
            townName.text = "Town Name: " + SelectedTown.name;
            populationCountText.text = "Town Population: " + SelectedTown.Population;
            listenerCountTextTown.text = "Listener Count: " + SelectedTown.Listeners;
            cultistCountTextTown.text = "Cultist Count: " + SelectedTown.Cultists;
            convertabilityLevelText.text = "Convertability Level: " + SelectedTown.Convertability + "%";
            preferredMusicTypeText.text = "Preferred Music Type: " + SelectedTown.PreferredMusicType.ToString();
            preferredAdTypeText.text = "Prefered Ad Type: " + SelectedTown.PreferredAdType.ToString();
            preferredCultTypeText.text = "Preferred Cult Type: " + SelectedTown.PreferredCultMessageType.ToString();
            stationCostText.text = "Station Cost: " + SelectedTown.StationCost.ToString();
        }
    }

    public void SelectRadioStation(Station station) {
        SelectedStation = station;
        stationManager.enabled = station != null && SelectedTown != null;
        if (stationManager.enabled) {
            musicTypes.value = (int) SelectedStation.ChosenMusicType;
            adTypes.value = (int) SelectedStation.ChosenAdType;
            cultTypes.value = (int) SelectedStation.ChosenCultMessageType;
            marketingTypes.value = (int) SelectedStation.ChosenMarketingPlanType;
            musicEffectivePercentText.text = "Music Effectiveness: " + SelectedStation.MusicEffectivePercent.ToString() + "%";
            propEffectivePercentText.text = "Propaganda Effectiveness: " + SelectedStation.PropagandaEffectivePercent.ToString() + "%";
            employeeCountText.text = "Employee Count: " + SelectedStation.Employees.Count.ToString();
            musicTime.value = SelectedStation.MusicTimePercent;
            adTime.value = SelectedStation.AdTimePercent;
            cultTime.value = SelectedStation.CultTimePercent;
            moneyVsHappy.value = SelectedStation.MoneyVsHappiness;
        }
    }

    public void BuyStation() {
        if (SelectedTown.ContainedStation == null && MoneyCount >= SelectedTown.StationCost) {
            MoneyCount -= SelectedTown.StationCost;
            Station newStation = Instantiate(stationClone, SelectedTown.transform);
            SelectedTown.ContainedStation = newStation;
            newStation.StationUpkeep = SelectedTown.StationUpkeep;
            newStation.ContainingTown = SelectedTown;
            newStation.MusicEffectivePercent = 1;
            newStation.PropagandaEffectivePercent = 1;
            newStation.AdvertisingRevenuePercent = 1;
            SelectRadioStation(newStation);
            UpdateText();
            foreach(Town t in SelectedTown.UnlockTowns) {
                if (!UnlockedTowns.Contains(t)) {
                    UnlockedTowns.Add(t);
                }
            }
        }        
    }

    void PopulateMusicDropdown() {
        string[] musicNames = Enum.GetNames(typeof(MusicType));
        List<string> enumerableNames = new List<string>(musicNames);

        musicTypes.AddOptions(enumerableNames);
    }

    void PopulateAdDropdown() {
        string[] adNames = Enum.GetNames(typeof(AdType));
        List<string> enumerableNames = new List<string>(adNames);

        adTypes.AddOptions(enumerableNames);
    }

    void PopulateCultDropdown() {
        string[] cultNames = Enum.GetNames(typeof(CultMessageType));
        List<string> enumerableNames = new List<string>(cultNames);

        cultTypes.AddOptions(enumerableNames);
    }

    void PopulateMarketingDropdown() {
        string[] marketingNames = Enum.GetNames(typeof(MarketingPlanType));
        List<string> enumerableNames = new List<string>(marketingNames);

        marketingTypes.AddOptions(enumerableNames);
    }

    public void MusicDropdownIndexChange(int index) {
        SelectedStation.ChosenMusicType = (MusicType) index;
    }

    public void AdDropdownIndexChange(int index) {
        SelectedStation.ChosenAdType = (AdType) index;
    }

    public void CultDropdownIndexChange(int index) {
        SelectedStation.ChosenCultMessageType = (CultMessageType) index;
    }

    public void MarketingDropdownIndexChange(int index) {
        SelectedStation.ChosenMarketingPlanType = (MarketingPlanType) index;
    }

    public void MusicTimeChange(float value) {
        SelectedStation.MusicTimePercent = value ;
    }

    public void AdTimeChange(float value) {
        SelectedStation.AdTimePercent = value;
    }

    public void CultTimeChange(float value) {
        SelectedStation.CultTimePercent = value;
    }

    public void MoneyVsHappyChange(float value) {
        SelectedStation.MoneyVsHappiness = value;
    }

    public void HireEmployee() {
        HireEmployee(SelectedStation);
    }

    public void HireEmployee(Station stationToHireAt) {
        Employee freshMeat = Instantiate(employeeClone, stationToHireAt.transform);
        freshMeat.Wage = 250f;
        freshMeat.CareCost = 0f;
        freshMeat.Loyalty = 0f;
        freshMeat.Happiness = 50f;
        stationToHireAt.AddEmployee(freshMeat);
        employeeCountText.text = "Employee Count: " + stationToHireAt.Employees.Count.ToString();
    }

    public void FireEmployee() {
        FireEmployee(SelectedStation);
    }

    public void FireEmployee(Station stationToFireFrom) {
        if (stationToFireFrom.Employees.Count > 0) {
            stationToFireFrom.Employees.RemoveAt(0);
            employeeCountText.text = "Employee Count: " + stationToFireFrom.Employees.Count.ToString();
        }
    }
}
