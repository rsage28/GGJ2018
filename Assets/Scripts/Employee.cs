﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour {
    private float wage;
    private float careCost;
    private float loyalty;
    private float happiness;

    public float Wage {
        get { return wage; }
        set { wage = value; }
    }

    public float CareCost {
        get { return careCost; }
        set { careCost = value; }
    }

    public float Loyalty {
        get { return loyalty; }
        set { loyalty = value; }
    }

    public float Happiness {
        get { return happiness; }
        set { happiness = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}