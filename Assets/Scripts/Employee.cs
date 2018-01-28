using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour {
    private float wage;
    private float careCost;
    [Range(0, 100)]
    private float loyalty;
    [Range(0, 100)]
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
}
