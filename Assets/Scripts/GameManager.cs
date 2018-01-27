using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    private float timeScale;
    private float timeStep;

    private float listenerCount;
    private float cultistCount;
    private float moneyCount;

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

    // Use this for initialization
    void Start() {
        timeScale = 0;
        ListenerCount = 0f;
        CultistCount = 0f;
        MoneyCount = 20000f;
    }

    void Awake() {
        if (TimeTick == null) {
            TimeTick = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
