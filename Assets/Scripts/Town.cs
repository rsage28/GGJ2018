using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour {
    [SerializeField]
    private Station containedStation;
    [SerializeField]
    private int population;
    private int listeners;
    [SerializeField]
    private int cultists;
    [SerializeField]
    private float convertability;
    [SerializeField]
    private MusicType preferredMusicType;
    [SerializeField]
    private AdType preferredAdType;
    [SerializeField]
    private CultMessageType preferredCultMessageType;

    [SerializeField]
    private float radius;

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

    public float Convertability {
        get { return convertability; }
        set { convertability = value; }
    }

    public MusicType PreferredMusicType {
        get { return preferredMusicType; }
    }

    public AdType PreferredAdType {
        get { return preferredAdType; }
    }

    public CultMessageType PreferredCultMessageType {
        get { return preferredCultMessageType; }
    }

    public float Radius {
        get { return radius; }
    }

    void Start () {
	}
	
	void Update () {
	}

    void OnDrawGizmos() {
        //Gizmos.DrawSphere(transform.position, Radius);
    }
}
