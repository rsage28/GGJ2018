using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    private List<Town> towns;

    public List<Town> Towns {
        get { return towns; } 
        set { towns = value; }
    }
}
