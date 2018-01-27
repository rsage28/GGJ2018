using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour {
    private Station containedStation;
    private int population;
    private int listeners;
    private int cultists;
    private float koolAidMeterPercent;
    private MusicType preferredMusic;
    private AdType preferredAd;
    // effective cult type enum?

    public Station ContainedStation {
        get { return containedStation; }
        set { containedStation = value; }
    }

    public int Population {
        get { return population; }
        set { population = value; }
    }

    public int Listeners {
        get { return listeners; }
        set { listeners = value; }
    }

    public int Cultists {
        get { return cultists; }
        set { cultists = value; }
    }

    public float KoolAidMeterPercent {
        get { return koolAidMeterPercent; }
        set { koolAidMeterPercent = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
