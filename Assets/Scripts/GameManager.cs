using System.Collections;
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

    public Canvas HUD;
    public Canvas stationManager;
    public Canvas townInfo;

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
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Time.timeScale = 0;
        }

        if (Time.time >= nextTimeStep) {
            nextTimeStep = Time.time + TimeStep;
            TimeTick.Invoke();
            if (UnityEngine.Random.Range(0, 100) <= EventChance) {
                randomEvent();
            }
            UpdateText();
        }

        if (SelectedTown != null) {
            buyStationButton.interactable = (MoneyCount >= SelectedTown.StationCost && UnlockedTowns.Contains(SelectedTown));
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
                    }
                }
            }
        }
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
            musicTypes.value = (int) SelectedStation.Music;
            adTypes.value = (int) SelectedStation.Ad;
            cultTypes.value = (int) SelectedStation.Cult;
            marketingTypes.value = (int) SelectedStation.MarketingPlan;
            musicEffectivePercentText.text = "Music Effectiveness: " + SelectedStation.MusicEffectivePercent.ToString() + "%";
            propEffectivePercentText.text = "Propaganda Effectiveness: " + SelectedStation.PropagandaEffectivePercent.ToString() + "%";
            employeeCountText.text = "Employee Count: " + SelectedStation.Employees.Count.ToString();
            musicTime.value = SelectedStation.MusicTimePercent / 100;
            adTime.value = SelectedStation.AdTimePercent / 100;
            cultTime.value = SelectedStation.CultTimePercent / 100;
            moneyVsHappy.value = SelectedStation.MoneyVsHappiness / 100;
        }
    }

    public void BuyStation() {
        if (SelectedTown.ContainedStation == null && MoneyCount >= SelectedTown.StationCost) {
            MoneyCount -= SelectedTown.StationCost;
            Station newStation = Instantiate(stationClone, SelectedTown.transform);
            SelectedTown.ContainedStation = newStation;
            newStation.StationUpkeep = SelectedTown.StationUpkeep;
            newStation.ContainingTown = SelectedTown;
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
        SelectedStation.Music = (MusicType) index;
    }

    public void AdDropdownIndexChange(int index) {
        SelectedStation.Ad = (AdType) index;
    }

    public void CultDropdownIndexChange(int index) {
        SelectedStation.Cult = (CultMessageType) index;
    }

    public void MarketingDropdownIndexChange(int index) {
        SelectedStation.MarketingPlan = (MarketingPlanType) index;
    }

    public void MusicTimeChange(float value) {
        SelectedStation.MusicTimePercent = value * 100;
    }

    public void AdTimeChange(float value) {
        SelectedStation.AdTimePercent = value * 100;
    }

    public void CultTimeChange(float value) {
        SelectedStation.CultTimePercent = value * 100;
    }

    public void MoneyVsHappyChange(float value) {
        SelectedStation.MoneyVsHappiness = value * 100;
    }

    public void HireEmployee() {
        Employee freshMeat = Instantiate(employeeClone, SelectedStation.transform);
        freshMeat.Wage = 250f;
        freshMeat.CareCost = 0f;
        freshMeat.Loyalty = 0f;
        freshMeat.Happiness = 50f;
        SelectedStation.AddEmployee(freshMeat);
        employeeCountText.text = "Employee Count: " + SelectedStation.Employees.Count.ToString();
    }

    public void FireEmployee() {
        if (SelectedStation.Employees.Count > 0) {
            SelectedStation.Employees.RemoveAt(0);
            employeeCountText.text = "Employee Count: " + SelectedStation.Employees.Count.ToString();
        }
    }
}
