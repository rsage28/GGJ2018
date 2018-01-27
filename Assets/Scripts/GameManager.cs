using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    }

    void Awake() {
        GameCamera = FindObjectOfType<GameCamera>();
        Instance = this;
        if (TimeTick == null) {
            TimeTick = new UnityEvent();
        }
    }

    void Update() {
        if (Time.time >= nextTimeStep) {
            nextTimeStep = Time.time + TimeStep;
            TimeTick.Invoke();
            if (Random.Range(0, 100) <= EventChance) {
                RandomEvent();
            }
        }        
    }

    private void RandomEvent() {
        print("a random event happened");
    }

    public Town getNearestTown(Vector3 pos) {
        return Towns.OrderBy(t => (t.transform.position - pos).sqrMagnitude).FirstOrDefault();
    }
}
